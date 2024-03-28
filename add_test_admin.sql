USE tourism_db;

-- login: admin; password: admin;
INSERT INTO user(Name, Surname, Login, Birth, PasswordSHA256, UserRole_idUserRole)
VALUES ('admin', 'admin', 'admin', '0001-01-01','8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918', 1);