void System::Console::Write(Ref<System::String>  value)
{
	printf("%s", value->c_str());
}