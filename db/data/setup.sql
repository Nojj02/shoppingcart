\connect postgres

 -- shoppingcart Schema
CREATE SCHEMA "shoppingcart"
    AUTHORIZATION postgres;

-- item table
CREATE TABLE "shoppingcart"."item"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    --"version" bigint NOT NULL,
    --"event_type" CHARACTER VARYING(100) COLLATE pg_catalog."default"  NOT NULL,
    --"event" jsonb NOT NULL,
    "content" jsonb NOT NULL,
    -- "type" CHARACTER VARYING(255) COLLATE pg_catalog."default" NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "item_pkey" PRIMARY KEY ("db_id")
    --,CONSTRAINT "version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."item"
    OWNER to postgres;

-- cart table
CREATE TABLE "shoppingcart"."cart"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    -- "version" bigint NOT NULL,
    -- "event_type" CHARACTER VARYING(100) COLLATE pg_catalog."default"  NOT NULL,
    -- "event" jsonb NOT NULL,
    "content" jsonb NOT NULL,
    -- "type" CHARACTER VARYING(255) COLLATE pg_catalog."default" NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "cart_pkey" PRIMARY KEY ("db_id")
    -- ,CONSTRAINT "version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."cart"
    OWNER to postgres;

-- coupon table
CREATE TABLE "shoppingcart"."coupon"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    --"version" bigint NOT NULL,
    --"event_type" CHARACTER VARYING(100) COLLATE pg_catalog."default"  NOT NULL,
    --"event" jsonb NOT NULL,
    "content" jsonb NOT NULL,
    -- "type" CHARACTER VARYING(255) COLLATE pg_catalog."default" NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "coupon_pkey" PRIMARY KEY ("db_id")
    --,CONSTRAINT "version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."coupon"
    OWNER to postgres;