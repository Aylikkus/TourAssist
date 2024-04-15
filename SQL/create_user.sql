CREATE USER 'tourism_admin'@'localhost' 
IDENTIFIED BY "1234" ;

GRANT ALL PRIVILEGES ON `tourism_db`.* 
TO 'tourism_admin'@'localhost';

FLUSH PRIVILEGES;