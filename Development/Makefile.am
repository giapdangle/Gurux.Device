
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;TRACE" "-keyfile:Gurux.Device.snk"
ASSEMBLY = bin/Debug/Gurux.Device.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug/

GURUX_DEVICE_DLL_MDB_SOURCE=bin/Debug/Gurux.Device.dll.mdb
GURUX_DEVICE_DLL_MDB=$(BUILD_DIR)/Gurux.Device.dll.mdb
GURUX_DEVICE_DLL_CONFIG_SOURCE=app.config
COMMON_LOGGING_DLL_SOURCE=Common.Logging.dll
QUARTZ_DLL_SOURCE=Quartz.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=SharedLibs/ICSharpCode.SharpZipLib.dll

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:TRACE" "-keyfile:Gurux.Device.snk"
ASSEMBLY = bin/Release/Gurux.Device.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release/

GURUX_DEVICE_DLL_MDB=
GURUX_DEVICE_DLL_CONFIG_SOURCE=app.config
COMMON_LOGGING_DLL_SOURCE=Common.Logging.dll
QUARTZ_DLL_SOURCE=Quartz.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=SharedLibs/ICSharpCode.SharpZipLib.dll

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(GURUX_DEVICE_DLL_MDB) \
	$(GURUX_DEVICE_DLL_CONFIG) \
	$(COMMON_LOGGING_DLL) \
	$(QUARTZ_DLL) \
	$(ICSHARPCODE_SHARPZIPLIB_DLL)  

LINUX_PKGCONFIG = \
	$(GURUX_DEVICE_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	AccessMode.cs \
	CategoryStates.cs \
	Editor/GXDataIOSourceActionConverter.cs \
	Editor/GXDataIOSourceDescriptor.cs \
	Editor/GXDataIOSourceEditor.cs \
	Editor/GXDataIOSourceTypeConverter.cs \
	Editor/GXObjectTypeConverter.cs \
	Editor/GXTableColumnEditor.cs \
	Editor/GXValueCollectionEditor.cs \
	Editor/GXValueItem.cs \
	Editor/GXValueItemCollection.cs \
	Editor/GXZip.cs \
	Editor/IGXPartialRead.cs \
	Editor/PartialReadType.cs \
	Editor/ValueAccessAttribute.cs \
	Editor/ValueAccessType.cs \
	GXCategoryStatistics.cs \
	DisabledActions.cs \
	AllowedMediaTypesConverter.cs \
	Editor/GXCollectionPropertyDescriptor.cs \
	Editor/GXCommunicationAttribute.cs \
	Editor/GXCommunicationMessageAttribute.cs \
	Editor/GXDataIOSourceDialog.cs \
	Editor/GXDataIOSourceDialog.Designer.cs \
	Editor/GXDataIOSourceType.cs \
	Editor/GXDesigner.cs \
	Editor/GXInitialActionMessage.cs \
	Editor/GXLocalizedDescriptionAttribute.cs \
	Editor/GXPropertyPageType.cs \
	Editor/GXSettingItemAttribute.cs \
	Editor/GXTableColumnConverter.cs \
	Editor/InitialActionType.cs \
	Editor/UITextTypeEditor.cs \
	GenericList.cs \
	GuruxProtocolSearchAddIn.cs \
	GXDelegates.cs \
	GXDeviceCollection.cs \
	Editor/GXDataIOSource.cs \
	Editor/GXLocalizedNameAttribute.cs \
	Editor/GXProtocolAddIn.cs \
	Editor/GXToolboxItemAttribute.cs \
	Editor/GXWizardButtons.cs \
	Editor/IGXComponent.cs \
	Editor/IGXWizardPage.cs \
	GXDeviceGroupCollection.cs \
	GXDeviceGroup.cs \
	Editor/DisplayTypes.cs \
	GXCategoryCollection.cs \
	GXCategory.cs \
	Editor/GXDataIOSourceAttribute.cs \
	GXDevice.cs \
	Editor/GXFlagsEnumConverter.cs \
	Editor/GXIDGenerator.cs \
	Editor/GXNumberEnumeratorConverterAttribute.cs \
	Editor/GXNumberEnumeratorEditor.cs \
	GXDeviceList.cs \
	Editor/GXNoUITypeEditor.cs \
	GXDeviceStatistics.cs \
	GXDeviceType.cs \
	GXDeviceTypeCollection.cs \
	GXEvents.cs \
	GXKeepalive.cs \
	GXMediaType.cs \
	GXMediaTypeCollection.cs \
	GXMediaTypeEditor.cs \
	GXMonitorListener.cs \
	GXParameter.cs \
	GXPropertyCollection.cs \
	GXProperty.cs \
	Editor/GXSite.cs \
	Editor/GXReadMessage.cs \
	GXSchedule.cs \
	GXScheduleCollection.cs \
	GXScheduleJob.cs \
	GXScheduleListener.cs \
	GXScheduleStatistics.cs \
	GXSerializedDevice.cs \
	GXTable.cs \
	GXTableCollection.cs \
	Editor/GXTask.cs \
	Editor/GXTaskCollection.cs \
	Editor/GXTemplateManager.cs \
	Editor/GXTypeConverterNoExpand.cs \
	Editor/GXUITextEditor.cs \
	Editor/GXUserLevelAttribute.cs \
	Editor/GXWriteMessage.cs \
	GXTableColumnAttribute.cs \
	IGXPacketHandler.cs \
	MediaTypeCollectionPropertyDescriptor.cs \
	Properties/AssemblyInfo.cs \
	Properties/Resources.Designer.cs \
	PropertyStates.cs \
	Editor/UITextEditor.cs \
	Editor/UserLevelType.cs \
	GXPropertyStatistics.cs \
	ResetType.cs \
	ScheduleAction.cs \
	ScheduleRepeat.cs \
	ScheduleState.cs \
	GXMonitorJob.cs \
	States.cs \
	TableStates.cs \
	GXTableStatistics.cs 

DATA_FILES = 

RESOURCES = \
	Editor/GXDataIOSourceDialog.resx,Gurux.Device.Editor.GXDataIOSourceDialog.resources \
	Editor/GXUITextEditor.resx,Gurux.Device.Editor.GXUITextEditor.resources \
	Properties/Resources.resx,Gurux.Device.Properties.Resources.resources 

EXTRAS = \
	app.config \
	Gurux.Device.snk \
	gurux.device.pc.in 

REFERENCES =  \
	$(GURUXCOMMON_LIBS) \
	$(GURUXCOMMUNICATION_LIBS) \
	System \
	System.Core \
	System.Design \
	System.Drawing \
	System.Runtime.Serialization \
	System.Windows.Forms \
	System.Data \
	System.Xml

DLL_REFERENCES =  \
	Common.Logging.dll \
	Quartz.dll \
	SharedLibs/ICSharpCode.SharpZipLib.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

GURUX_DEVICE_DLL_CONFIG = $(BUILD_DIR)/Gurux.Device.dll.config
COMMON_LOGGING_DLL = $(BUILD_DIR)/Common.Logging.dll
QUARTZ_DLL = $(BUILD_DIR)/Quartz.dll
ICSHARPCODE_SHARPZIPLIB_DLL = $(BUILD_DIR)/ICSharpCode.SharpZipLib.dll
GURUX_DEVICE_PC = $(BUILD_DIR)/gurux.device.pc

$(eval $(call emit-deploy-target,GURUX_DEVICE_DLL_CONFIG))
$(eval $(call emit-deploy-target,COMMON_LOGGING_DLL))
$(eval $(call emit-deploy-target,QUARTZ_DLL))
$(eval $(call emit-deploy-target,ICSHARPCODE_SHARPZIPLIB_DLL))
$(eval $(call emit-deploy-wrapper,GURUX_DEVICE_PC,gurux.device.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
