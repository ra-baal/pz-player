/*
* Records a point cloud from a kinect and save to a file.
*/

#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this

#include <iostream>
#include <string>

#include <pcl/ModelCoefficients.h>

#include <pcl/io/pcd_io.h>
#include <pcl/io/ply_io.h>
#include <pcl/io/boost.h>
#include <pcl/io/openni2_grabber.h>
#include <pcl/io/openni2/openni2_device_manager.h>

#include <pcl/point_types.h>

#include <pcl/filters/extract_indices.h>
#include <pcl/filters/voxel_grid.h>

#include <pcl/features/normal_3d.h>

#include <pcl/kdtree/kdtree.h>

#include <pcl/sample_consensus/method_types.h>
#include <pcl/sample_consensus/model_types.h>

#include <pcl/segmentation/sac_segmentation.h>
#include <pcl/segmentation/extract_clusters.h>

#include <pcl/visualization/cloud_viewer.h>

#include <pcl/common/time.h>


//using namespace boost;

// Link following functions C-style (required for plugins)
extern "C" {

	class KinectCloudGrabberRGBA
	{
		pcl::Grabber* interface;
		bool viewerWasStopped;

	public:
		pcl::PointCloud<pcl::PointXYZRGBA>::ConstPtr cloudFromKinect;

		KinectCloudGrabberRGBA() : viewerWasStopped(false) /*, cloudFromKinect (new pcl::PointCloud<pcl::PointXYZ>) */
		{
		}

		void cloud_cb_(const pcl::PointCloud<pcl::PointXYZRGBA>::ConstPtr& cloud)
		{
			if (!viewerWasStopped)
			{
				cloudFromKinect = cloud;
				viewerWasStopped = true;
			}
		}

		pcl::PointCloud<pcl::PointXYZRGBA>::ConstPtr grabCloudRGBA()
		{
			//pcl::Grabber* interface = new pcl::OpenNIGrabber(); 
			pcl::Grabber* interface = new pcl::io::OpenNI2Grabber();

			std::function<void(const pcl::PointCloud<pcl::PointXYZRGBA>::ConstPtr&)> f = boost::bind(
				&KinectCloudGrabberRGBA::cloud_cb_, this, boost::placeholders::_1);

			interface->registerCallback(f);
			interface->start();

			while (!viewerWasStopped)
			{
				std::this_thread::sleep_for(std::chrono::seconds(1));
			}

			interface->stop();

			return cloudFromKinect;
		}

	};

	EXPORT_API bool readKinectCloudRGBA(pcl::PointCloud<pcl::PointXYZRGBA>::Ptr& output)
	{
		KinectCloudGrabberRGBA kinectCloudGrabber;
	
		pcl::PointCloud<pcl::PointXYZRGBA>::ConstPtr cloud = kinectCloudGrabber.grabCloudRGBA();
	
		// std::cout << "Cloudsize :" << cloud->points.size () <<  std::endl;

		if (cloud->empty())
		{
			return false;
		}

		// Create the filtering object: downsample the dataset using a leaf size of 1cm

		pcl::VoxelGrid<pcl::PointXYZRGBA> vg;
		vg.setInputCloud(cloud);
		vg.setLeafSize(0.01f, 0.01f, 0.01f);
		vg.filter(*output);

		return true;
	}

	int main()
	{
		pcl::PointCloud<pcl::PointXYZRGBA>::Ptr maincloudRGBA(new pcl::PointCloud<pcl::PointXYZRGBA>);

		std::string fileName;
		cout << "Enter the filename to save (without extension): ";
		cin >> fileName;

		try 
		{
			pcl::io::openni2::OpenNI2DeviceManager manager;
			pcl::io::openni2::OpenNI2Device::Ptr device = manager.getAnyDevice();
			cout << "Device name: " << device->getName() << endl;

			device->setSynchronization(true);
			cout << "Synchronization set." << endl;

			readKinectCloudRGBA(maincloudRGBA);
			cout << "Cloud read." << endl;
			
			pcl::io::savePCDFileASCII(fileName + ".ascii.pcd", *maincloudRGBA);
			cout << "PCD ASCII saved." << endl;
			pcl::io::savePCDFileBinary(fileName + ".binary.pcd", *maincloudRGBA);
			cout << "PCD binary saved." << endl;
			pcl::io::savePLYFileASCII(fileName + ".ascii.ply", *maincloudRGBA);
			cout << "PLY ASCII saved." << endl;
			pcl::io::savePLYFileBinary(fileName + ".binary.ply", *maincloudRGBA);
			cout << "PLY binary saved." << endl;

		}
		catch (std::exception &e)
		{
			cout << e.what() << endl;
		}

		cout << "Press enter to exit..." ;

		getchar();
		return 0;
	}

} // end of export C block
