

IF EXIST %~dp0importdata.db del %~dp0importdata.db
IF EXIST %~dp0importfiles.txt del %~dp0importfiles.txt
IF EXIST %~dp0locationsimported.db del %~dp0locationsimported.db

echo CREATE TABLE Addresses("UPRN","RM_UDPRN","CHANGE_TYPE","STATE","STATE_DATE","CLASS","PARENT_UPRN","X_COORDINATE","Y_COORDINATE","RPC","LOCAL_CUSTODIAN_CODE","START_DATE","END_DATE","LAST_UPDATE_DATE","ENTRY_DATE","ORGANISATION_NAME","BLPU_ORGANISATION","DEPARTMENT_NAME","SCOTTISH_DEPARTMENT_NAME","BUILDING_NAME","SUB_BUILDING_NAME","SAO_START_NUMBER","SAO_START_SUFFIX","SAO_END_NUMBER","SAO_END_SUFFIX","SAO_TEXT","ALT_LANGUAGE_SAO_TEXT","PAO_START_NUMBER","PAO_START_SUFFIX","PAO_END_NUMBER","PAO_END_SUFFIX","PAO_TEXT","ALT_LANGUAGE_PAO_TEXT","USRN","USRN_MATCH_INDICATOR","AREA_NAME","LEVEL","OFFICIAL_FLAG","OS_ADDRESS_TOID","OS_ADDRESS_TOID_VERSION","OS_ROADLINK_TOID","OS_ROADLINK_TOID_VERSION","OS_TOPO_TOID","OS_TOPO_TOID_VERSION","VOA_CT_RECORD","VOA_NDR_RECORD","STREET_DESCRIPTION","ALT_LANGUAGE_STREET_DESCRIPTOR","DEPENDENT_THOROUGHFARE","THOROUGHFARE","WELSH_DEPENDENT_THOROUGHFARE","WELSH_THOROUGHFARE","DOUBLE_DEPENDENT_LOCALITY","DEPENDENT_LOCALITY","LOCALITY","WELSH_DEPENDENT_LOCALITY","WELSH_DOUBLE_DEPENDENT_LOCALITY","TOWN_NAME","ADMINISTRATIVE_AREA","POST_TOWN","POSTCODE","POSTCODE_LOCATOR","POSTCODE_TYPE","POSTAL_ADDRESSABLE","PO_BOX_NUMBER","WARD_CODE","PARISH_CODE","PROCESS_DATE","MULTI_OCC_COUNT","VOA_NDR_P_DESC_CODE","VOA_NDR_SCAT_CODE","ALT_LANGUAGE"); >>%~dp0importfiles.txt

echo .echo on >>%~dp0importfiles.txt

echo .separator "," >>%~dp0importfiles.txt
setlocal EnableDelayedExpansion

for /r "%~1" %%i in (*.csv) do (
set str=%%i
call set newstr=!str:\=\\!
echo .import !newstr! Addresses>>%~dp0importfiles.txt
)

%~dp0sqlite3 %~dp0importdata.db < %~dp0importfiles.txt

echo file data imported

echo copying data to correct structure

%~dp0sqlite3 %~dp0locationsimported.db < %~dp0CreateLocationsDb.txt

echo creating lucene index and importing data
GoatTrip.LucenceIndexer.exe s %~dp0locationsimported.db i %~dp0index





