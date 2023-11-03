USE appReservasDB;
--USUARIOS
--tbCredenciales
DROP TABLE tbCredenciales;
CREATE TABLE tbCredenciales (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario VARCHAR(30) NOT NULL,
    hash VARCHAR(255) NOT NULL
);

--tbContactos
DROP TABLE tbContactos
CREATE TABLE tbContactos(
	id INT IDENTITY(1,1) PRIMARY KEY, 
	email VARCHAR(30) NOT NULL,
	phone VARCHAR(15) NOT NULL
);

--tbPermisos
DROP TABLE tbPermisos
CREATE TABLE tbPermisos(
	id INT IDENTITY(1,1) PRIMARY KEY,
	tipoUsuario VARCHAR(20) NOT NULL
);

--permisos basicos
INSERT INTO tbPermisos (tipoUsuario)VALUES('cliente');
INSERT INTO tbPermisos (tipoUsuario)VALUES('establecimiento');
INSERT INTO tbPermisos (tipoUsuario)VALUES('adminUsr');

--tbUsuario
DROP TABLE tbUsuario;
CREATE TABLE tbUsuario(
	id INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(30) NOT NULL,
	aMaterno VARCHAR(30) NOT NULL,
	aPaterno VARCHAR(30) NOT NULL,
	idContacto INT NOT NULL,
	idCredenciales INT NOT NULL,
	idPermisos INT NOT NULL,
	FOREIGN KEY (idContacto) REFERENCES tbContactos(id),
	FOREIGN KEY (idCredenciales) REFERENCES tbCredenciales(id),
	FOREIGN KEY (idPermisos) REFERENCES tbPermisos(id)
);

SELECT * FROM tbUsuario;

--procedimiento crear usuario
GO
CREATE PROCEDURE sp_CrearUsuario
    @nombre VARCHAR(30),
    @aMaterno VARCHAR(30),
    @aPaterno VARCHAR(30),
    @email VARCHAR(30),
    @phone VARCHAR(15),
    @usuario VARCHAR(30),
    @password VARCHAR(255), -- Assuming you'll pass a hashed password
    @tipoUsuario VARCHAR(20)
AS
BEGIN
    DECLARE @idContacto INT;
    DECLARE @idCredenciales INT;
    DECLARE @idPermisos INT;

    -- Insert email and phone into tbContactos
    INSERT INTO tbContactos (email, phone)
    VALUES (@email, @phone);

    -- Get the ID of the inserted contact
    SET @idContacto = SCOPE_IDENTITY();

    -- Create a hash based on usuario and password
    -- You should use a secure hash function, such as SHA-256
    -- You need to adapt this hashing logic according to your security requirements
    -- For this example, we'll use a simple concatenation of usuario and password
    SET @password = HASHBYTES('SHA2_256', @usuario + @password);

    -- Insert user and hash into tbCredenciales
    INSERT INTO tbCredenciales (usuario, hash)
    VALUES (@usuario, @password);

    -- Get the ID of the inserted credentials
    SET @idCredenciales = SCOPE_IDENTITY();

    -- Retrieve the ID of the user's permission based on the provided @tipoUsuario
    SELECT @idPermisos = id
    FROM tbPermisos
    WHERE tipoUsuario = @tipoUsuario;

    -- Insert the user with the rest of the fields and foreign keys
    INSERT INTO tbUsuario (nombre, aMaterno, aPaterno, idContacto, idCredenciales, idPermisos)
    VALUES (@nombre, @aMaterno, @aPaterno, @idContacto, @idCredenciales, @idPermisos);
END;

--test sp_CrearUsuario
-- Sample data
DECLARE @nombre VARCHAR(30) = 'John';
DECLARE @aMaterno VARCHAR(30) = 'Doe';
DECLARE @aPaterno VARCHAR(30) = 'Smith';
DECLARE @email VARCHAR(30) = 'john.doe@example.com';
DECLARE @phone VARCHAR(15) = '123-456-7890';
DECLARE @usuario VARCHAR(30) = 'johndoe';
DECLARE @password VARCHAR(255) = 'mysecretpassword';
DECLARE @tipoUsuario VARCHAR(20) = 'cliente';

-- Execute the stored procedure
EXEC sp_CrearUsuario @nombre, @aMaterno, @aPaterno, @email, @phone, @usuario, @password, @tipoUsuario;

-- Check the result (you can query the tables to verify the data)
SELECT * FROM tbUsuario;


--HOTELES
--tbTipoHabitacion
DROP TABLE tbTipoHabitacion;
CREATE TABLE tbTipoHabitacion(
	id INT IDENTITY(1,1) PRIMARY KEY,
	tipo VARCHAR(30)
);
--tipos basicos de habitacion
INSERT INTO tbTipoHabitacion (tipo)VALUES('económica');
INSERT INTO tbTipoHabitacion (tipo)VALUES('standar');
INSERT INTO tbTipoHabitacion (tipo)VALUES('suite');

--tbCamas
DROP TABLE tbCamas;
CREATE TABLE tbCamas(
	id INT IDENTITY(1,1) PRIMARY KEY,
	disposicion VARCHAR(30)
);

--tipos basicos de acomodación 
INSERT INTO tbCamas (disposicion) VALUES('sencilla');
INSERT INTO tbCamas (disposicion) VALUES('doble');
INSERT INTO tbCamas (disposicion) VALUES('triple');

--tbHoteles
DROP TABLE tbHotel;
CREATE TABLE tbHotel(
	id INT IDENTITY(1,1) PRIMARY KEY,
	nombre VARCHAR(30),
	direccion VARCHAR(50),
	idContacto INT,
	--idHabitaciones INT
);

SELECT * FROM tbHotel;

--procedimiento crear hotel
DROP PROCEDURE sp_PopulateHotel;
GO
CREATE PROCEDURE sp_PopulateHotel
    @nombre VARCHAR(30),
    @direccion VARCHAR(50),
    @email VARCHAR(30),
    @phone VARCHAR(15)
AS
BEGIN
    DECLARE @contactID INT;

    -- Insert data into tbContactos and retrieve the generated id
    INSERT INTO tbContactos (email, phone)
    VALUES (@email, @phone);

    -- Retrieve the generated id
    SELECT @contactID = SCOPE_IDENTITY();

    -- Insert data into tbHotel using the retrieved contactID
    INSERT INTO tbHotel (nombre, direccion, idContacto)
    VALUES (@nombre, @direccion, @contactID);
END;


--test sp_PopulateHotel
-- Sample data
DECLARE @nombre VARCHAR(30) = 'Sample Hotel';
DECLARE @direccion VARCHAR(50) = '123 Main Street, Sample City';
DECLARE @email VARCHAR(30) = 'hotel@example.com';
DECLARE @phone VARCHAR(15) = '555-123-4567';

-- Execute the stored procedure
EXEC sp_PopulateHotel @nombre, @direccion, @email, @phone;

-- Check the result (you can query the tables to verify the data)
SELECT * FROM tbHotel;


--tbHabitaciones
DROP TABLE tbHabitaciones;
CREATE TABLE tbHabitaciones(
	id INT IDENTITY(1,1) PRIMARY KEY,
	idHotel INT,
	idTipoHabitacion INT,
	idCamas INT
	FOREIGN KEY (idHotel) REFERENCES tbHotel(id),
	FOREIGN KEY (idTipoHabitacion) REFERENCES tbTipoHabitacion(id),
	FOREIGN KEY (idCamas) REFERENCES tbCamas(id)
);

--procedimiento crear habitacion
GO
CREATE PROCEDURE sp_PopulateHabitacion
    @idHotel INT,
    @idTipoHabitacion INT,
    @idCamas INT
AS
BEGIN
    -- Check if @idHotel exists in the tbHotel table
    IF NOT EXISTS (SELECT 1 FROM tbHotel WHERE id = @idHotel)
    BEGIN
        -- Hotel with the specified id doesn't exist; return an error
        RAISERROR('Hotel with the specified ID does not exist.', 16, 1);
        RETURN;
    END;

    -- Check if @idTipoHabitacion exists in the tbTipoHabitacion table
    IF NOT EXISTS (SELECT 1 FROM tbTipoHabitacion WHERE id = @idTipoHabitacion)
    BEGIN
        -- Room type with the specified id doesn't exist; return an error
        RAISERROR('Room type with the specified ID does not exist.', 16, 2);
        RETURN;
    END;

    -- Check if @idCamas exists in the tbCamas table
    IF NOT EXISTS (SELECT 1 FROM tbCamas WHERE id = @idCamas)
    BEGIN
        -- Bed configuration with the specified id doesn't exist; return an error
        RAISERROR('Bed configuration with the specified ID does not exist.', 16, 3);
        RETURN;
    END;

    -- All parameters are valid; create a new room
    INSERT INTO tbHabitaciones (idHotel, idTipoHabitacion, idCamas)
    VALUES (@idHotel, @idTipoHabitacion, @idCamas);
END;

--test sp_PopulateHabitacion
-- Sample data
DECLARE @idHotel INT = 1; -- Assuming idHotel for the hotel is 1
DECLARE @idTipoHabitacion INT = 1; -- Assuming idTipoHabitacion for 'económica' is 1
DECLARE @idCamas INT = 1; -- Assuming idCamas for 'sencilla' is 1

-- Execute the stored procedure to populate tbHabitaciones
EXEC sp_PopulateHabitacion @idHotel, @idTipoHabitacion, @idCamas;

-- Check the result (you can query the tbHabitaciones table to verify the data)
SELECT * FROM tbHabitaciones;

--trigger para poblar matriz bloqueo
DROP TRIGGER tr_CreateMatrizBloqueo;
GO
CREATE TRIGGER tr_CreateMatrizBloqueo
ON tbHabitaciones
AFTER INSERT
AS
BEGIN
    DECLARE @HabitacionID INT;
    DECLARE @Year INT;
    DECLARE @DayOfYear INT = 1;

    -- Get the newly inserted habitacion id
    SELECT @HabitacionID = id FROM inserted;

    -- Set the year for the reservations
    SET @Year = YEAR(GETDATE());

    -- Insert into tbMatrizBloqueo
    INSERT INTO tbMatrizBloqueo (idHabitacion, reservationYear)
    VALUES (@HabitacionID, @Year);

    -- Insert into tbMatrizBloqueoDetalle for the entire year
    WHILE @DayOfYear <= 365
    BEGIN
        INSERT INTO tbMatrizBloqueoDetalle (idMatrizBloqueo, dayOfYear, isAvailable)
        VALUES ((SELECT id FROM tbMatrizBloqueo WHERE idHabitacion = @HabitacionID AND reservationYear = @Year), @DayOfYear, 0);

        SET @DayOfYear = @DayOfYear + 1;
    END;
END;



--tbReserva
DROP TABLE tbReserva;
CREATE TABLE tbReserva(
	id INT IDENTITY(1,1) PRIMARY KEY,
	idUsuario INT,
	idHabitacion INT,
	Pago VARCHAR(20),
	fechaIngreso DATE,
	fechaSalida DATE,
	FOREIGN KEY (idUsuario) REFERENCES tbUsuario(id),
	FOREIGN KEY (idHabitacion) REFERENCES tbHabitaciones(id)
);

select * from tbReserva;



--tbMatrizBloqueoDetalle
DROP TABLE tbMatrizBloqueoDetalle;
CREATE TABLE tbMatrizBloqueoDetalle (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idMatrizBloqueo INT,
    dayOfYear INT, -- Day of the year (1-365)
    isAvailable BIT, -- 0 for unavailable, 1 for available
    FOREIGN KEY (idMatrizBloqueo) REFERENCES tbMatrizBloqueo(id)
);

SELECT * FROM tbMatrizBloqueoDetalle;

--tbMatriz de Bloqueo
DROP TABLE tbMatrizBloqueo;
CREATE TABLE tbMatrizBloqueo (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idHabitacion INT,
    reservationYear INT -- Include a column for the year
);

SELECT * FROM tbMatrizBloqueo;

--procedimiento crear reserva
--1. sp chequear disponibilidad
DROP PROCEDURE sp_CheckRoomAvailability;
GO
CREATE PROCEDURE sp_CheckRoomAvailability
    @idHabitacion INT,
    @fechaIngreso DATE,
    @fechaSalida DATE
AS
BEGIN
    DECLARE @Year INT;
    DECLARE @IsAvailable INT = 1;
    DECLARE @UnavailableDates NVARCHAR(MAX) = N'';

    -- Calculate the days of the year for the reservation period
    DECLARE @StartDate DATE = @fechaIngreso;
    DECLARE @EndDate DATE = @fechaSalida;
    DECLARE @DayOfYear INT = DATEPART(DAYOFYEAR, @StartDate);

    -- Get the current year
    SET @Year = YEAR(GETDATE());

    -- Print initial values for debugging
    PRINT 'Start Date: ' + CAST(@StartDate AS VARCHAR(10));
    PRINT 'End Date: ' + CAST(@EndDate AS VARCHAR(10));
    PRINT 'DayOfYear: ' + CAST(@DayOfYear AS VARCHAR(5));
    PRINT 'Year: ' + CAST(@Year AS VARCHAR(4));

    -- Check the availability for each day within the reservation period
    WHILE @StartDate <= @EndDate
    BEGIN
        -- Print the date being checked
        PRINT 'Checking Date: ' + CAST(@StartDate AS VARCHAR(10));

        -- Check if the room is reserved for the specific day
        SELECT @IsAvailable = ISNULL(
            (SELECT isAvailable
             FROM tbMatrizBloqueoDetalle AS D
             JOIN tbMatrizBloqueo AS M ON D.idMatrizBloqueo = M.id
             WHERE M.idHabitacion = @idHabitacion
               AND M.reservationYear = @Year
               AND D.dayOfYear = @DayOfYear),
            1  -- Default to 1 (unavailable) if no record is found
        );

        -- Print the value of IsAvailable for debugging
        PRINT 'IsAvailable for Day ' + CAST(@DayOfYear AS VARCHAR(5)) + ': ' + CAST(@IsAvailable AS VARCHAR(1));

        -- If any day is occupied, add it to the list of unavailable dates
        IF @IsAvailable = 1
        BEGIN
            IF @UnavailableDates <> ''
                SET @UnavailableDates += ', ';
            SET @UnavailableDates += CAST(@StartDate AS NVARCHAR(10));
        END;

        -- Move to the next day
        SET @StartDate = DATEADD(DAY, 1, @StartDate);
        SET @DayOfYear = @DayOfYear + 1;
    END;

    -- Check the availability result and return the appropriate value
    IF @UnavailableDates = ''
    BEGIN
        PRINT 'All dates are available.';
        SELECT 0 AS IsAvailable, '' AS UnavailableDates;
    END
    ELSE
    BEGIN
        PRINT 'Unavailable Dates: ' + @UnavailableDates;
        SELECT 1 AS IsAvailable, @UnavailableDates AS UnavailableDates;
    END;
END;




--test sp_CheckAvailability
-- Sample test parameters
DECLARE @idHabitacion INT = 2;       -- Replace with a valid room ID
DECLARE @fechaIngreso DATE = '2023-11-01'; -- Check-in date
DECLARE @fechaSalida DATE = '2023-11-10';  -- Check-out date

-- Execute the stored procedure to check room availability
EXEC sp_CheckRoomAvailability @idHabitacion, @fechaIngreso, @fechaSalida;

-- Check the result (1 for availability, 0 for unavailability)
SELECT 'Room Availability Status' = CASE WHEN @@ROWCOUNT = 1 THEN 1 ELSE 0 END;



--2. bloquear fechas
DROP PROCEDURE sp_BlockRoomDates;
GO
CREATE PROCEDURE sp_BlockRoomDates
    @idHabitacion INT,
    @fechaIngreso DATE,
    @fechaSalida DATE
AS
BEGIN
    DECLARE @Year INT;
    DECLARE @DayOfYear INT;
    DECLARE @MatrizBloqueoId INT;
    DECLARE @StartDate DATE;
    DECLARE @EndDate DATE;

    -- Calculate the days of the year for the reservation period
    SET @StartDate = @fechaIngreso;
    SET @EndDate = @fechaSalida;
    SET @DayOfYear = DATEPART(DAYOFYEAR, @StartDate);

    -- Get the current year
    SET @Year = YEAR(GETDATE());

    -- Find the MatrizBloqueo ID
    SELECT @MatrizBloqueoId = id
    FROM tbMatrizBloqueo
    WHERE idHabitacion = @idHabitacion AND reservationYear = @Year;

    -- DEBUG: Print the variables
    PRINT 'Start Date: ' + CONVERT(VARCHAR, @StartDate);
    PRINT 'End Date: ' + CONVERT(VARCHAR, @EndDate);
    PRINT 'DayOfYear: ' + CONVERT(VARCHAR, @DayOfYear);
    PRINT 'Year: ' + CONVERT(VARCHAR, @Year);

    -- Loop through the days within the reservation period
    WHILE @StartDate <= @EndDate
    BEGIN
        -- DEBUG: Print the checking date
        PRINT 'Checking Date: ' + CONVERT(VARCHAR, @StartDate);

        -- Update the availability in tbMatrizBloqueoDetalle
        UPDATE tbMatrizBloqueoDetalle
        SET isAvailable = 1
        WHERE idMatrizBloqueo = @MatrizBloqueoId AND dayOfYear = @DayOfYear;

        -- DEBUG: Print the updated date
        PRINT 'Updated Date: ' + CONVERT(VARCHAR, @StartDate);

        -- DEBUG: Print the affected rows
        SELECT * FROM tbMatrizBloqueoDetalle WHERE idMatrizBloqueo = @MatrizBloqueoId AND dayOfYear = @DayOfYear;

        -- Move to the next day
        SET @StartDate = DATEADD(DAY, 1, @StartDate);
        SET @DayOfYear = @DayOfYear + 1;
    END;

    -- Return a success code
    SELECT 1 AS ResultCode;
END;



-- Variables for testing
DECLARE @idHabitacion INT = 2;  -- Replace with a valid room ID
DECLARE @fechaIngreso DATE = '2023-11-01';
DECLARE @fechaSalida DATE = '2023-11-05';

-- Execute the sp_BlockRoomDates procedure
EXEC sp_BlockRoomDates @idHabitacion, @fechaIngreso, @fechaSalida;

--3. crear reserva
DROP PROCEDURE sp_MakeReservation;
GO
CREATE PROCEDURE sp_MakeReservation
    @idUsuario INT,
    @idHabitacion INT,
    @Pago VARCHAR(20),
    @fechaIngreso DATE,
    @fechaSalida DATE
AS
BEGIN
    BEGIN TRY
        -- Start a transaction
        BEGIN TRANSACTION;

        -- Check if the user exists
        IF NOT EXISTS (SELECT 1 FROM tbUsuario WHERE id = @idUsuario)
        BEGIN
            -- Rollback the transaction and return 0 (User does not exist)
            ROLLBACK;
            SELECT 0 AS ReservationStatus;
            RETURN;
        END;

        -- Check if the room exists
        IF NOT EXISTS (SELECT 1 FROM tbHabitaciones WHERE id = @idHabitacion)
        BEGIN
            -- Rollback the transaction and return 0 (Room does not exist)
            ROLLBACK;
            SELECT 0 AS ReservationStatus;
            RETURN;
        END;

        -- Check room availability
        DECLARE @Availability INT;
        EXEC @Availability = sp_CheckRoomAvailability @idHabitacion, @fechaIngreso, @fechaSalida;
        IF @Availability = 0
        BEGIN
            -- Room is not available
            -- Rollback the transaction and return 0 (Room is not available)
            ROLLBACK;
            SELECT 0 AS ReservationStatus;
            RETURN;
        END;

        -- Block room dates
        EXEC sp_BlockRoomDates @idHabitacion, @fechaIngreso, @fechaSalida;

        -- Insert the reservation
        INSERT INTO tbReserva (idUsuario, idHabitacion, Pago, fechaIngreso, fechaSalida)
        VALUES (@idUsuario, @idHabitacion, @Pago, @fechaIngreso, @fechaSalida);

        -- Commit the transaction
        COMMIT;

        -- Return 1 to indicate a successful reservation
        SELECT 1 AS ReservationStatus;
    END TRY
    BEGIN CATCH
        -- An error occurred, rollback the transaction and return 0
        ROLLBACK;
        SELECT 0 AS ReservationStatus;
    END CATCH;
END;

--test sp_MakeReservation
-- Sample test parameters
DECLARE @idUsuario INT = 1;          -- Replace with a valid user ID
DECLARE @idHabitacion INT = 2;       -- Replace with a valid room ID
DECLARE @Pago VARCHAR(20) = 'Cash';  -- Payment method
DECLARE @fechaIngreso DATE = '2023-11-10'; -- Check-in date
DECLARE @fechaSalida DATE = '2023-11-15';  -- Check-out date

-- Execute the stored procedure to make a reservation
EXEC sp_MakeReservation @idUsuario, @idHabitacion, @Pago, @fechaIngreso, @fechaSalida;

-- Check the result (1 for success, 0 for failure)
SELECT 'Reservation Status' = CASE WHEN @@ROWCOUNT = 1 THEN 1 ELSE 0 END;







