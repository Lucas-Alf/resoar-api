DROP FUNCTION IF EXISTS query_research;

CREATE OR REPLACE FUNCTION query_research(
    p_query varchar,
    p_start_year int,
    p_final_year int,
    p_type int[],
    p_language varchar[],
    p_institution int[],
    p_authors int[],
    p_advisors int[],
    p_keywords int[],
    p_knowledge_areas int[],
    p_limit int,
    p_offset int
)
RETURNS TABLE (
    "id" int,
    "title" varchar(350),
    "year" int,
    "type" int,
    "languageName" varchar(80),
    "fileKey" uuid,
    "thumbnailKey" uuid,
    "institutionId" int,
    "createdAt" timestamp,
    "abstract" varchar(5000),
    "rank" float4
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
        "Language" AS "languageName", 
        "FileKey" AS "fileKey", 
        "ThumbnailKey" AS "thumbnailKey", 
        "InstitutionId" AS "institutionId",
        "CreatedAt" AS "createdAt",
        "Abstract" AS "abstract",
        (
            CASE WHEN p_query IS NOT NULL
                THEN ts_rank_cd("SearchVector", websearch_to_tsquery("Language"::regconfig, p_query))
                ELSE 0
            END
        ) AS "Rank"
    FROM "Research" AS res
        WHERE res."Visibility" = 1
        AND (CASE WHEN p_start_year IS NOT NULL THEN "Year" >= p_start_year ELSE TRUE END)
        AND (CASE WHEN p_final_year IS NOT NULL THEN "Year" <= p_final_year ELSE TRUE END)
        AND (CASE WHEN p_type IS NOT NULL THEN "Type" = ANY(p_type) ELSE TRUE END)
        AND (CASE WHEN p_language IS NOT NULL THEN "Language" = ANY(p_language) ELSE TRUE END)
        AND (CASE WHEN p_institution IS NOT NULL THEN "InstitutionId" = ANY(p_institution) ELSE TRUE END)
        AND (
            CASE WHEN p_query IS NOT NULL
                THEN ts_rank_cd("SearchVector", websearch_to_tsquery("Language"::regconfig, p_query)) > 0
                ELSE TRUE
            END
        )
        AND (
            CASE WHEN p_authors IS NOT NULL
                THEN EXISTS(SELECT rat."Id" FROM "ResearchAuthor" AS rat WHERE rat."ResearchId" = res."Id" AND rat."UserId" = ANY(p_authors))
                ELSE TRUE
            END
        )
        AND (
            CASE WHEN p_advisors IS NOT NULL
                THEN EXISTS(SELECT rad."Id" FROM "ResearchAdvisor" AS rad WHERE rad."ResearchId" = res."Id" AND rad."UserId" = ANY(p_advisors))
                ELSE TRUE
            END
        )
        AND (
            CASE WHEN p_keywords IS NOT NULL
                THEN EXISTS(SELECT kwo."Id" FROM "ResearchKeyWord" AS kwo WHERE kwo."ResearchId" = res."Id" AND kwo."KeyWordId" = ANY(p_keywords))
                ELSE TRUE
            END
        )
        AND (
            CASE WHEN p_knowledge_areas IS NOT NULL
                THEN EXISTS(SELECT kwa."Id" FROM "ResearchKnowledgeArea" AS kwa WHERE kwa."ResearchId" = res."Id" AND kwa."KnowledgeAreaId" = ANY(p_knowledge_areas))
                ELSE TRUE
            END
        )
    ORDER BY "Rank" DESC, res."Title" ASC
        LIMIT p_limit
        OFFSET p_offset;
END;
$BODY$
LANGUAGE 'plpgsql';