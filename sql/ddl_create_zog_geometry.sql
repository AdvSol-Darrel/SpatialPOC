-- gis_test.zog_geometry definition

-- Drop table

-- DROP TABLE gis_test.zog_geometry;

CREATE TABLE gis_test.zog_geometry (
	id numeric NOT NULL,
	geometry geometry NULL,
	srid int4 NULL,
	geometry_type varchar NULL
);
CREATE INDEX zog_geometry_id_idx ON gis_test.zog_geometry (id);