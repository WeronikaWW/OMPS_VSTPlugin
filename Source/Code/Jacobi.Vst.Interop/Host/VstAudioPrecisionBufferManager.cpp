#include "pch.h"
#include "VstAudioPrecisionBufferManager.h"
#include "..\Properties\Resources.h"

namespace Jacobi {
namespace Vst {
namespace Host {
namespace Interop {

	VstAudioPrecisionBufferManager::VstAudioPrecisionBufferManager(System::Int32 bufferCount, System::Int32 bufferSize)
	{
		if(bufferCount < 0)
		{
			throw gcnew System::ArgumentOutOfRangeException("bufferCount");
		}

		if(bufferSize <= 0)
		{
			throw gcnew System::ArgumentOutOfRangeException("bufferSize");
		}

		_bufferCount = bufferCount;
		_bufferSize = bufferSize;

		_managedBuffers = gcnew System::Collections::Generic::List<Jacobi::Vst::Core::VstAudioPrecisionBuffer^>();

		if(_bufferCount > 0)
		{
			// allocate the buffers in one call
			double* pBuffer = _unmanagedBuffers.GetArray(bufferCount * bufferSize);
			ClearAllBuffers();

			for(int n = 0; n < bufferCount; n++)
			{
				double* pRunning = pBuffer + (bufferSize * n);
				_managedBuffers->Add(gcnew Jacobi::Vst::Core::VstAudioPrecisionBuffer(pRunning, bufferSize, true));
			}
		}
	}

	VstAudioPrecisionBufferManager::~VstAudioPrecisionBufferManager()
	{
		// destroys the contained UnmanagedArray.
	}

	void VstAudioPrecisionBufferManager::ClearBuffer(Jacobi::Vst::Core::VstAudioPrecisionBuffer^ buffer)
	{
		Jacobi::Vst::Core::Throw::IfArgumentIsNull(buffer, "buffer");

		if(buffer->SampleCount != _bufferSize)
		{
			throw gcnew System::ArgumentException(
				Jacobi::Vst::Interop::Properties::Resources::VstAudioBufferManager_InvalidBufferSize, "buffer");
		}
		
		auto directBuf = (Jacobi::Vst::Core::IDirectBufferAccess64^)buffer;

		double* lowerBound = _unmanagedBuffers.GetArray();
		double* upperBound = lowerBound + (_bufferSize * _bufferCount);
		double* pBuffer = directBuf->Buffer;

		// check if the unmanaged buffer matches the range of our _unamangedBuffers array.
		if(lowerBound == NULL ||
			!(lowerBound <= pBuffer && pBuffer < upperBound))
		{
			throw gcnew System::ArgumentException(
				Jacobi::Vst::Interop::Properties::Resources::VstAudioBufferManager_BufferNotOwned, "buffer");
		}

		ClearBuffer(directBuf->Buffer, directBuf->SampleCount);
	}

	void VstAudioPrecisionBufferManager::ClearAllBuffers()
	{
		ClearBuffer(_unmanagedBuffers.GetArray(), _unmanagedBuffers.GetLength());
	}

}}}} // Jacobi::Vst::Host::Interop
