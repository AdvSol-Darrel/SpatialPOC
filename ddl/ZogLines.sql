-- gis_test.zog_lines definition

-- Drop table

-- DROP TABLE gis_test.zog_lines;

CREATE TABLE gis_test.zog_lines (
	line_id numeric NULL,
	geometry geometry NULL
);

CREATE INDEX zog_lines_line_id_idx ON gis_test.zog_lines (line_id);