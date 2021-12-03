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
    noteType INT NOT NULL,
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
    FOREIGN KEY (noteType)
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
	('12345678Z', HASHBYTES('MD5', '1234'), 1),
	('12345679W', HASHBYTES('MD5', '1234'), 2),
	('11111111Z', HASHBYTES('MD5', '1234'), 3),
	('12345678A', HASHBYTES('MD5', '1234'), 4),
	('12345678S', HASHBYTES('MD5', '1234'), 5),
	('12345678C', HASHBYTES('MD5', '1234'), 6),
	('12345678B', HASHBYTES('MD5', '1234'), 7);

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

CREATE VIEW Fullemployee AS
	SELECT 
	emp.id AS employeeId,
	CONCAT(COALESCE(emp.name,''),' ',COALESCE(emp.surname1,''),' ',COALESCE(emp.surname2,'')) AS employeeName 
	from employee emp 
	INNER JOIN employee_range emra 
	ON emp.typeId = emra.id 
	WHERE emra.id <> 2

GO

CREATE VIEW technician AS
	SELECT 
	emp.id AS employeeId,
	CONCAT(COALESCE(emp.name,''),' ',COALESCE(emp.surname1,''),' ',COALESCE(emp.surname2,'')) AS employeeName 
	from employee emp 
	INNER JOIN employee_range emra 
	ON emp.typeId = emra.id 
	WHERE emra.id <> 1

GO

CREATE VIEW FullIncidence AS
	SELECT inc.id AS id, 
	inc.ownerId AS employeeId, 
	emp.employeeName AS employeeName, 
	note.noteStr AS issueDesc, 
	inc.solverId AS technicianId, 
	tec.employeeName AS technicianName,
	inc.open_dateTime AS issueDateTime, 
	inc.close_dateTime AS resolutionDateTime,
	inc.state AS state
	FROM incidence inc 
	INNER JOIN Fullemployee emp 
	ON emp.employeeId = inc.ownerId 
	LEFT JOIN technician tec 
	ON inc.solverId = tec.employeeId
	INNER JOIN notes note
	ON note.incidenceId = inc.id AND note.noteTypeId = 1
	WHERE inc.state <> 5

GO

CREATE VIEW FullPiece AS
	SELECT pc.id,
	pc.name,
	pc.typeId AS pieceTypeId,
	pt.name As pieceTypeName,
	pt.description AS pieceTypeDescription,
	pc.deleted
	FROM piece_class pc
	INNER JOIN piece_type pt
	ON pc.typeId = pt.id;

GO

CREATE VIEW completeEmployee AS
	SELECT 
	emp.id, 
	emp.dni, 
	emp.name AS name,
	emp.surname1 AS surname1,
	emp.surname2 AS surname2,
	CONCAT(COALESCE(emp.name, ''), ' ', COALESCE(emp.surname1, ''), ' ', COALESCE(emp.surname2, '')) as employeeName, 
	emp.typeId AS typeId, 
	emra.id AS typeRangeId,
	emra.name AS typeRange,
	emp.state AS deleted 
	FROM employee emp
	INNER JOIN employee_range emra
	ON emp.typeId = emra.id

GO

CREATE VIEW incidence_pieces AS
	SELECT 
	ipl.incidenceId AS incidenceId, 
	pc.id AS pieceId, 
	pc.typeId AS typeId,
	pt.name AS typeName, 
	pt.description AS typeDescription,
	pc.name AS name, 
	pc.deleted AS deleted 
	FROM piece_class pc 
	INNER JOIN incidence_piece_log ipl 
	ON pc.id = ipl.pieceId 
	INNER JOIN piece_type pt
	ON pc.typeId = pt.id
	WHERE ipl.status = 0

GO

CREATE VIEW FullNote AS
	SELECT nt.Id, 
	nt.employeeId, 
	nt.incidenceId, 
	nt.noteTypeId, 
	noty.name AS noteTypeName,
	nt.noteStr, 
	nt.date
	FROM Notes nt
	INNER JOIN note_type noty
	ON nt.noteTypeId = noty.id

GO

CREATE VIEW incidence_notes AS
	SELECT 
	nt.incidenceId,
	nt.noteStr AS noteStr,
	nt.date as noteDate,
	noty.name AS noteType
	FROM Notes nt 
	INNER JOIN incidence inc 
	ON inc.id = nt.incidenceId
	INNER JOIN note_type noty
	ON nt.noteTypeId = noty.id

GO

CREATE VIEW reportedPieces AS
	SELECT 
	pc.name AS pieceName, 
	count(ipl.pieceId) AS pieceNumber 
	FROM incidence_piece_log ipl 
	INNER JOIN piece_class pc 
	ON ipl.pieceId = pc.id 
	INNER JOIN incidence inc 
	ON ipl.incidenceId = inc.id 
	WHERE inc.state IN (3,4) 
	AND ipl.status = 0 
	GROUP BY ipl.pieceId, pc.name

GO

CREATE VIEW credentialsMatch AS 
	SELECT C.*
	FROM credentials C INNER JOIN employee E
	ON E.id=C.employeeId
	WHERE E.state=0

GO

