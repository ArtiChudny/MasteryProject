This is console file storage application.
To login application you should run application with parameters --l for login and --p for password.

login:		storageUser
password:	storagePassword

Max size of file is 2GB
Max capacity of storage is 5GB
You should to create folder "Storage" which is located in the same directory as the executable file, and create folder "Files" inside folder "Storage".

Сurrently available commands:
user info 						- Getting information about user.

directory create "dirPath" "directoryName"		- Create subdirectory in destination path. Examle: directory create "/root" "music"
directory info "dirPath"				- Get information about selected directory
directory move "oldDirPath" "newDirPath"		- Move or rename your directory. Example: directory move "/root/music" "/root/films/videoclips" 
directory remove "dirPath"				- Delete directory from storage, Example: file remove "/root/films/videoclips"
directory list "dirPath"				- Get information about inner directories and files in selected directory
directory search "dirPath" "searchLine"			- Get list of all directories and files in selected directory that match searchline. Example:  directory search "/root" "searchLine"

file upload "filePath" "storageDestinationPath" 	- Upload file into your storage. Example: file upload "C:\Users\User\Documets\file.txt" "/root/documents"
file download "fileStoragePath" "destinationPath" 	- Download file from your repository to destination path. Example: file upload "/root/documents/file.txt" "C:\Users\User\Documets" 
file move "oldStorageFilePath" "newStorageFilePath"	- Move or rename your file in storage. Example: file move "/root/documents/file.txt" "/root/lectures/newfile.txt" 
file remove "storageFilePath"				- Delete file from storage, Example: file remove "/root/lectures/newfile.txt"
file info "storageFilePath" 				- Getting information about file into storage
file export --info					- Getting list of available export formats
file export "destinationPath" 				- Export meta-information about the storage in some format (json is default if flag --format was not used)
				--format json			
				--format xml

!!!Use quotes "" for command parameters!!!