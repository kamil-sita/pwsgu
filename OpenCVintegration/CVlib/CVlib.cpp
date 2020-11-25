// CVlib.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "CVlib.h"


// This is an example of an exported variable
CVLIB_API int nCVlib=0;

// This is an example of an exported function.
CVLIB_API int fnCVlib(void)
{
    return 0;
}

// This is the constructor of a class that has been exported.
CCVlib::CCVlib()
{
    return;
}

unsigned char* matToByte(cv::Mat mat) {
    return mat.data;
}

cv::Mat byteToMat(long long length, int width, int height, unsigned char* data, int type = CV_8UC3)
{
   cv::Mat ImMat = cv::Mat(height, width, type, data);
   cv::flip(ImMat, ImMat, -1);
   return ImMat;
}

bool detectFace(long long length, int width, int height, unsigned char* data, float& tlx, float&
    tly, float& brx, float& bry)
    {
    cv::Mat img = byteToMat(length, width, height, data, CV_8UC3);
    std::vector <cv::Rect > objects;
    loadClassifier();
    faceClassifier.detectMultiScale(img, objects);
    int i = 0;
    for (auto it = objects.begin(); it != objects.end(); ++it, i++)
    {
        tlx = it->x;
        tly = it->y;
        brx = it->width;
        bry = it->height;
    }

    return 1;
    }

bool loadClassifier() {
    faceClassifier.load("../../opencv/build/etc/haarcascades/haarcascade_frontalface_default.xml");
    return(1);
}
