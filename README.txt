This is console file storage application.
To login application you should run application with parameters --l for login and --p for password.

login:		storageUser
password:	storagePassword

Max size of file is 2GB
Max capacity of storage is 5GB
You should to create folder "Storage" which is located in the same directory as the executable file, and create folder "Files" inside folder "Storage".

Сurrently available commands:
user info 					- getting information about user.
file upload "filePath" 				- upload file into your storage
file download "fileName" "destinationPath" 	- download file from your repository to destination path
file move "oldFileName" "newFileName"		- rename your file in storage
file remove "fileName"				- delete file from storage
file info "fileName" 				- getting information about file into storage

!!!Use quotes "" for command parameters!!!