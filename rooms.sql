-- Процедура добавления новой комнаты
CREATE OR REPLACE PROCEDURE add_room(
    p_number VARCHAR,
    p_area DECIMAL,
    p_department_id INTEGER
)
AS $$
BEGIN
    INSERT INTO "Rooms"(number, area, department_id)
    VALUES (p_number, p_area, p_department_id);
END;
$$ LANGUAGE plpgsql;

-- Процедура обновления информации о комнате
CREATE OR REPLACE PROCEDURE update_room(
    p_number VARCHAR,
    p_area DECIMAL,
    p_department_id INTEGER
)
AS $$
BEGIN
    UPDATE "Rooms"
    SET area = p_area,
        department_id = p_department_id
    WHERE number = p_number;
END;
$$ LANGUAGE plpgsql;

-- Процедура удаления комнаты
CREATE OR REPLACE PROCEDURE delete_room(p_number VARCHAR)
AS $$
BEGIN
    DELETE FROM "Rooms" WHERE number = p_number;
END;
$$ LANGUAGE plpgsql;