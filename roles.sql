-- Процедура добавления новой роли
CREATE OR REPLACE PROCEDURE add_role(
    p_name VARCHAR
)
AS $$
BEGIN
    INSERT INTO "Roles"(name)
    VALUES (p_name);
END;
$$ LANGUAGE plpgsql;

-- Процедура обновления информации о роли
CREATE OR REPLACE PROCEDURE update_role(
    p_id INTEGER,
    p_name VARCHAR
)
AS $$
BEGIN
    UPDATE "Roles"
    SET name = p_name
    WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- Процедура удаления роли
CREATE OR REPLACE PROCEDURE delete_role(p_id INTEGER)
AS $$
BEGIN
    DELETE FROM "Roles" WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;