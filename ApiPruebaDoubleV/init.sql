CREATE DATABASE PruebaDoubleV;

CREATE TABLE IF NOT EXISTS tbl_tickets (
    Id SERIAL PRIMARY KEY,
    Usuario VARCHAR(100) NOT NULL,
    FechaCreacion TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    FechaActualizacion TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    Estatus BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE OR REPLACE FUNCTION ObtenerTickets(
    p_usuario VARCHAR(100),
    p_page INTEGER DEFAULT 1,
    p_page_size INTEGER DEFAULT 10
)
RETURNS TABLE (
    Id INT,
    Usuario VARCHAR(100),
    FechaCreacion TIMESTAMP WITH TIME ZONE,
    FechaActualizacion TIMESTAMP WITH TIME ZONE,
    Estatus BOOLEAN
)
AS $$
BEGIN
    RETURN QUERY
    SELECT t.Id, t.Usuario, t.FechaCreacion, t.FechaActualizacion, t.Estatus
    FROM tbl_tickets t
    WHERE (p_usuario IS NULL OR t.Usuario ILIKE '%' || p_usuario || '%')
    ORDER BY t.Id DESC
    LIMIT p_page_size OFFSET (p_page - 1) * p_page_size;
END;
$$ LANGUAGE plpgsql;


-- Crear función para insertar un ticket
CREATE OR REPLACE FUNCTION CrearTicket(
    p_usuario VARCHAR(100)
)
RETURNS INTEGER AS $$
DECLARE
    nuevo_id INTEGER;
BEGIN
    INSERT INTO tbl_tickets (Usuario, FechaCreacion, FechaActualizacion)
    VALUES (p_usuario, NOW(), NOW())
    RETURNING Id INTO nuevo_id;

    RETURN nuevo_id;
END;
$$ LANGUAGE plpgsql;

-- Función para editar ticket (solo usuario y estatus)
CREATE OR REPLACE FUNCTION editarTicket(
    p_id INT,
    p_usuario VARCHAR,
    p_estatus BOOLEAN
)
RETURNS VOID AS $$
BEGIN
    UPDATE tbl_tickets
    SET usuario = p_usuario,
        estatus = p_estatus,
        fechaactualizacion = NOW()
    WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;


-- Función para eliminar ticket
CREATE OR REPLACE FUNCTION eliminarTicket(
    p_id INT
)
RETURNS VOID AS $$
BEGIN
    DELETE FROM tbl_tickets WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;