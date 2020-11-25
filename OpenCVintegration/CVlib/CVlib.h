// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the CVLIB_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// CVLIB_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef CVLIB_EXPORTS
#define CVLIB_API __declspec(dllexport)
#else
#define CVLIB_API __declspec(dllimport)
#endif

#include <opencv2/opencv.hpp>
extern "C" {
	cv::Mat byteToMat(long long length, int width, int height, unsigned char* data, int type);
	unsigned char* matToByte(cv::Mat mat);
	CVLIB_API bool detectFace(long long length, int width, int height, unsigned char* data,
		float& tlx, float& tly, float& brx, float& bry);
	CVLIB_API bool loadClassifier();
}

// This class is exported from the dll
class CVLIB_API CCVlib {
public:
	CCVlib(void);
	// TODO: add your methods here.
};

cv::CascadeClassifier faceClassifier;

extern CVLIB_API int nCVlib;

CVLIB_API int fnCVlib(void);
