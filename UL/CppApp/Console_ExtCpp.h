void System::Console::Write(Ref<System::String>  value)
{
	wprintf(_T("%s"), value->c_str());
}