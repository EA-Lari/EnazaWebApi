
DO $$
SETOF INT
BEGIN
  CREATE TEMP TABLE tmp_a ( VALUE INT);
  CREATE TEMP TABLE tmp_b ( VALUE INT);
  INSERT INTO tmp_a (VALUE) values (10);
  INSERT INTO tmp_b (VALUE) VALUES (100);
    IF EXISTS(SELECT 1 from tmp_a) THEN
        Return QUERY  (select * from tmp_a);
    ELSE
        IF EXISTS(SELECT 1 from tmp_b) THEN
            Return QUERY  (SELECT * FROM tmp_b);
        ELSE
            Return QUERY (SELECT NULL);
        END IF;
    END IF;
END $$;



