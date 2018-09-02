\connect postgres

 -- shoppingcart Schema
CREATE SCHEMA "shoppingcart"
    AUTHORIZATION postgres;

-- ride table
CREATE TABLE "shoppingcart"."shop"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    --"version" bigint NOT NULL,
    --"event_type" CHARACTER VARYING(100) COLLATE pg_catalog."default"  NOT NULL,
    --"event" jsonb NOT NULL,
    "content" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "shop_pkey" PRIMARY KEY ("db_id")
    --,CONSTRAINT "version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."shop"
    OWNER to postgres;