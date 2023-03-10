#pragma once

class Utils
{
public:
	static void ShowWarning(System::String^ message);
	static void ShowError(System::Exception^ e);

	static System::String^ GetCurrentFileName()
	{ return System::Reflection::Assembly::GetExecutingAssembly()->Location; }

	static System::String^ GetPluginName()
	{ return System::IO::Path::GetFileNameWithoutExtension(GetCurrentFileName()); }
};
