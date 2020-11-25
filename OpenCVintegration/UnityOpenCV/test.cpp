#include <opencv2/imgproc.hpp>
#include <opencv2/objdetect.hpp>
#include <opencv2/highgui.hpp>
#include <opencv2/videoio.hpp>
#include <opencv2/video.hpp>
#include <iostream>

int main() {
	cv::VideoCapture vc;
	vc.open(0);
	cv::CascadeClassifier faceClassifier;
	faceClassifier.load("../opencv/build/etc/haarcascades/haarcascade_frontalface_default.xml");
	for (;;) {
		cv::Mat img;
		vc >> img;
		std::vector <cv::Rect > objects;
		faceClassifier.detectMultiScale(img, objects);

		//cv::imshow(" test ", img);
		cv::waitKey(1);
		int i = 0;
		int tlx;
		int tly;
		int brx;
		int bry;
		for (auto it = objects.begin(); it != objects.end(); ++it, i++)
		{
			tlx = it->x;
			tly = it->y;
			brx = it->width;
			bry = it->height;
			cv::Rect rect(tlx, tly, brx, bry);
			cv::rectangle(img, rect, cv::Scalar(0, 255, 0));
			cv::imshow(" test ", img);
			std::cout << "[tlx , tly , brx , bry ]=[ " << tlx << "," << tly << "," << brx << "," << bry << "]\n";
		}

		//cv::imshow(" test ", img);
		std::cout << " ============================\ n";

	}

}