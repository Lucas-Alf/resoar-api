DROP FUNCTION IF EXISTS query_research;

CREATE OR REPLACE FUNCTION query_research(p_query VARCHAR)
RETURNS TABLE (
    "id" int,
    "title" varchar(350),
    "year" int,
    "type" int,
    "visibility" int,
    "language" varchar(80),
    "fileKey" varchar(350),
    "thumbnailKey" varchar(350),
    "institutionId" int,
    "createdAt" timestamp,
    "rank" float4,
    "headline" text
) 
AS
$BODY$
BEGIN
RETURN QUERY 
    SELECT 
        "Id" AS "id", 
        "Title" AS "title",
        "Year" AS "year",
        "Type" AS "type",
        "Visibility" AS "visibility", 
        "Language" AS "language", 
        "fileKey" AS "fileKey", 
        "thumbnailKey" AS "thumbnailKey", 
        "InstitutionId" AS "institutionId",
        "CreatedAt" AS "createdAt",
        ts_rank_cd("SearchVector", websearch_to_tsquery("Language"::regconfig, p_query)) AS "rank",
        ts_headline("Language"::regconfig, "RawContent", websearch_to_tsquery("Language"::regconfig, p_query)) AS "headline"
    FROM "Research"
        ORDER BY "rank" DESC;
END;
$BODY$
LANGUAGE 'plpgsql';