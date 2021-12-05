CREATE TABLE employee_range (
    id INT PRIMARY KEY NOT NULL IDENTITY,
    name VARCHAR(64) NOT NULL
);

CREATE TABLE employee (
    id INT PRIMARY KEY NOT NULL IDENTITY,
    dni VARCHAR(9) UNIQUE NOT NULL,
    name VARCHAR(100) NOT NULL,
    surname1 VARCHAR (100) NOT NULL,
    surname2 VARCHAR (100),
    typeId INT NOT NULL,
    state INT NOT NULL DEFAULT 0
    CONSTRAINT employee_range_id
    FOREIGN KEY (typeId)
    REFERENCES employee_range (id)
);

INSERT INTO employee_range (name) VALUES
	('Employee'),
	('Technician'),
	('Admin');

CREATE TABLE state (
    id INT PRIMARY KEY NOT NULL IDENTITY,
    name VARCHAR(50) NOT NULL
);

INSERT INTO state (name) VALUES 
	('Nuevo'),
	('En curso'),
	('Cerrado');

CREATE TABLE incidence (
    id INT PRIMARY KEY IDENTITY,
    ownerId INT NOT NULL,
    solverId INT,
    open_dateTime dateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
	close_dateTime datetime NULL,
    state INT DEFAULT 1 NOT NULL,
    CONSTRAINT incidence_employee
    FOREIGN KEY (ownerId)
    REFERENCES employee (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
    CONSTRAINT incidence_technician
    FOREIGN KEY (solverId)
    REFERENCES employee (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
    CONSTRAINT incidence_state
    FOREIGN KEY (state)
    REFERENCES state (id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
);

CREATE TABLE piece_type (
	id INT PRIMARY KEY NOT NULL IDENTITY,
	name VARCHAR(100) NOT NULL,
	description VARCHAR(500)
);

INSERT INTO piece_type (name, description) VALUES 
	('Interno', 'Componentes relativos al interior de la torre (fuente, placa base, procesador memoria principal, memorias secundarias, etc.)'),
	('Externo', 'Componentes periféricos (monitor, impresora, teclado, ratón, etc.)'),
	('Otros', 'Componentes adicionales (pendrive, pincho wifi, cables eléctricos, regletas, pilas, etc.)');

CREATE TABLE piece_class (
	id INT PRIMARY KEY NOT NULL IDENTITY,
	name VARCHAR(100) NOT NULL,
    typeId INT NOT NULL,
    deleted TINYINT DEFAULT 0,
	CONSTRAINT pieceTypeId
    FOREIGN KEY (typeId)
    REFERENCES piece_type (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

INSERT INTO piece_class (typeId, name) VALUES 
	(1, 'RAM'),
	(1, 'HDD o SSD'),
	(1, 'Placa base'),
	(1, 'Tarjeta Gráfica (GPU)'),
	(2, 'Ratón'),
	(2, 'Teclado'),
	(2, 'Impresora'),
	(2, 'Cámara'),
	(1, 'Refrigeración (ventiladores, disipadores...)'),
	(1, 'Procesador (CPU)'),
	(2, 'Monitor'),
	(3, 'Otros');

CREATE TABLE incidence_piece_log (
	id INT PRIMARY KEY NOT NULL IDENTITY,
	pieceId INT NOT NULL,
	incidenceId INT NOT NULL,
    status INT DEFAULT 0,
	CONSTRAINT piece_id
    FOREIGN KEY (pieceId)
    REFERENCES piece_class (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
	CONSTRAINT incidence_id
    FOREIGN KEY (incidenceId)
    REFERENCES incidence (id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

CREATE TABLE note_type (
    id INT PRIMARY KEY NOT NULL IDENTITY,
    name VARCHAR(100) NOT NULL
);

INSERT INTO note_type (name) VALUES
	('ownerNote'),
	('solverNote');

 CREATE TABLE Notes (
    Id INT PRIMARY KEY NOT NULL IDENTITY,
    employeeId INT NOT NULL,
    incidenceId INT NOT NULL,
    noteTypeId INT NOT NULL,
    noteStr VARCHAR(200) NOT NULL,
    date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT notes_employee
    FOREIGN KEY (employeeId)
    REFERENCES employee (id)
    ON UPDATE CASCADE
    ON DELETE CASCADE,
    CONSTRAINT notes_incidence
    FOREIGN KEY (incidenceId)
    REFERENCES incidence (id)
    ON UPDATE CASCADE
    ON DELETE CASCADE,
    CONSTRAINT notes_type
    FOREIGN KEY (noteTypeId)
    REFERENCES note_type (id)
    ON UPDATE CASCADE
    ON DELETE CASCADE
 );

  CREATE TABLE Credentials(
	Id INT PRIMARY KEY NOT NULL IDENTITY,
	username VARCHAR(100) UNIQUE NOT NULL,
	password VARCHAR(100) NOT NULL,
	employeeId INT NOT NULL,
	CONSTRAINT credentials_employee
    FOREIGN KEY (employeeId)
    REFERENCES employee (id)
    ON UPDATE CASCADE
    ON DELETE CASCADE
 );

 INSERT INTO employee (dni, name, surname1, surname2, typeId) VALUES 
	('12345678Z', 'Jose Javier', 'Valero', 'Fuentes', 2),
	('12345679Z', 'Juan Francisco', 'Navarro', 'Ramiro', 2),
	('11111111Z', 'Jose', 'admin', 'istrador', 3),
	('12345678A', 'Jose', 'jackson', 'arzapalo', 1),
	('12345678S', 'Jose Antonio', 'Lidon', 'Ferrer', 1),
	('12345678C', 'Samuel', 'Garcia', 'Sanchez', 1),
	('12345678B', 'jessie', 'deep', NULL, 1);

INSERT INTO credentials (username, password, employeeId) VALUES 
	('12345678Z', '81dc9bdb52d04dc20036dbd8313ed055', 1),
	('12345679W', '81dc9bdb52d04dc20036dbd8313ed055', 2),
	('11111111Z', '81dc9bdb52d04dc20036dbd8313ed055', 3),
	('12345678A', '81dc9bdb52d04dc20036dbd8313ed055', 4),
	('12345678S', '81dc9bdb52d04dc20036dbd8313ed055', 5),
	('12345678C', '81dc9bdb52d04dc20036dbd8313ed055', 6),
	('12345678B', '81dc9bdb52d04dc20036dbd8313ed055', 7);

CREATE LOGIN Ad WITH PASSWORD = 'p@$$w0rd';
GRANT ALL PRIVILEGES ON Incidences to Ad;

GO

CREATE VIEW Tiempo_resolucion 
AS
SELECT ((((YEAR(inc.close_dateTime))-(YEAR(inc.open_dateTime)))*31536000)+
	(((MONTH(inc.close_dateTime))-(MONTH(inc.open_dateTime)))*2592000)+
	(((DAY(inc.close_dateTime))-(DAY(inc.open_dateTime)))*86400)+
	(((DATEPART(hour, inc.close_dateTime))-(DATEPART(hour, inc.open_dateTime)))*3600)+
	(((DATEPART(minute, inc.close_dateTime))-(DATEPART(minute, inc.open_dateTime)))*60)+
	((DATEPART(second, inc.close_dateTime))-(DATEPART(second, inc.open_dateTime)))) AS "Tiempo", 
	inc.id, inc.solverId, CONCAT(e.name, ' ', e.surname1, ' ', e.surname2) AS employeeName
	FROM incidence inc
	INNER JOIN employee e
	ON inc.solverId = e.id
	WHERE inc.state IN (3, 4)
	GROUP BY inc.id, inc.solverId, inc.close_dateTime, inc.open_dateTime, e.name, e.surname1, e.surname2;
GO