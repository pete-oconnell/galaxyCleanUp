# galaxyCleanUp
This is a basic C# application which will perform a single run through of all objects and should there be any templates which does not have anything derived will be deleted unless the template name exists in the proctectedObjects.txt file

# Usage
!! Ensure sufficient backups are implemented prior to running this application !!

The application when being executed will request for the galaxy host, name, username & password. If your galaxy does not implement security simply leaving the username and password blank is acceptable.
The application runs on a single pass of all objects, finding templates without any instances or templates derived from them and will look to delete them if they're not listed in the protected objects file.
The application will report how many templates it has deleted, when it can no longer delete any unused templates the application will report 0 objects deleted.

Unsupported
-----------

The primary implications of being UNSUPPORTED are:

1. This code has had LIMITED testing and may not work completely as intended and may have unintended side effects.
1. This includes NO WARRANTY OF ANY KIND. AVEVA assumes NO responsibility for this code or any unintended consequences of using them.
1. By using this code, you assume FULL responsibility for the consequences.
1. The code may fail to work following a product update (patch, service pack, major release) that makes changes to existing database objects.
1. Wonderware/AVEVA assumes no responsibility to answer questions or assist with the use of the code themselves (although, to the degree they leverage standard product features, those are of course supported).
1. Requests of support for this code should be raised via an issue on github which it will be investigated in due course