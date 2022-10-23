DROP FUNCTION IF EXISTS query_research;

CREATE OR REPLACE FUNCTION query_research(p_query varchar, p_limit int, p_offset int)
RETURNS TABLE (
    "id" int,
    "title" varchar(350),
    "year" int,
    "type" int,
    "visibility" int,
    "language" varchar(80),
    "fileKey" uuid,
    "thumbnailKey" uuid,
    "institutionId" int,
    "createdAt" timestamp,
    "abstract" varchar(2500),
    "rank" float4
) 
AS
$BODY$
BEGIN
RETURN QUERY
    SELECT * FROM 
    (
        SELECT 
            "Id" AS "id", 
            "Title" AS "title",
            "Year" AS "year",
            "Type" AS "type",
            "Visibility" AS "visibility", 
            "Language" AS "language", 
            "FileKey" AS "fileKey", 
            "ThumbnailKey" AS "thumbnailKey", 
            "InstitutionId" AS "institutionId",
            "CreatedAt" AS "createdAt",
            "Abstract" AS "abstract",
            ts_rank_cd("SearchVector", websearch_to_tsquery("Language"::regconfig, p_query)) AS "rank"
        FROM "Research"
    ) AS query
    WHERE query."rank" > 0
    ORDER BY query."rank" DESC
    LIMIT p_limit
    OFFSET p_offset;
END;
$BODY$
LANGUAGE 'plpgsql';