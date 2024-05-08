USE tourism_db;

INSERT INTO Transport(Name, Capacity) VALUES 
('Маршрутное такси', 20), ('Автобус (маленький)', 25),
('Автобус (средний)', 50), ('Автобус (большой)', 80),
('Самолёт (маленький)', 100), ('Самолёт (средний)', 250),
('Самолёт (большой)', 500), ('Пассажирское судно (маленькое)', 12),
('Пассажирское судно (среднее)', 50), ('Пассажирское судно (большое)', 100);
INSERT INTO UserRole(Name) VALUES ('admin'), ('guest');

-- login: admin; password: admin;
INSERT INTO user(Name, Surname, Login, Birth, PasswordSHA256, UserRole_idUserRole)
VALUES ('admin', 'admin', 'admin', '0001-01-01','8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918', 1);