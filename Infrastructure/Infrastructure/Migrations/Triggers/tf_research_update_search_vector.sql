CREATE OR REPLACE FUNCTION tf_research_update_search_vector()
  RETURNS trigger AS
$BODY$
BEGIN
	NEW."SearchVector" = (SELECT to_tsvector(NEW."Language"::regconfig, NEW."RawContent"));
RETURN NEW;
END;
$BODY$
LANGUAGE 'plpgsql';

CREATE OR REPLACE TRIGGER after_insert_search_vector
  BEFORE INSERT
  ON "Research"
  FOR EACH ROW
  EXECUTE PROCEDURE tf_research_update_search_vector();
  
CREATE OR REPLACE TRIGGER after_update_research_search_vector
  BEFORE INSERT
  ON "Research"
  FOR EACH ROW
  EXECUTE PROCEDURE tf_research_update_search_vector();