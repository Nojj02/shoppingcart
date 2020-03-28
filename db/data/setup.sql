\connect postgres

 -- shoppingcart Schema
CREATE SCHEMA "shoppingcart"
    AUTHORIZATION postgres;

-- item table
CREATE TABLE "shoppingcart"."item_type"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "version" bigint NOT NULL,
    "event_type" CHARACTER VARYING(255) COLLATE pg_catalog."default"  NOT NULL,
    "event" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "item_type_pkey" PRIMARY KEY ("db_id")
    ,CONSTRAINT "item_type_version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."item_type"
    OWNER to postgres;

-- item table
CREATE TABLE "shoppingcart"."item"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "version" bigint NOT NULL,
    "event_type" CHARACTER VARYING(255) COLLATE pg_catalog."default"  NOT NULL,
    "event" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "item_pkey" PRIMARY KEY ("db_id")
    ,CONSTRAINT "item_version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."item"
    OWNER to postgres;

-- event tracking table
CREATE TABLE "shoppingcart"."event_tracking"
(
    "db_id" bigserial NOT NULL,
    "resource_name" CHARACTER VARYING(255) COLLATE pg_catalog."default" NOT NULL,
    "last_message_number" int NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "event_tracking_pkey" PRIMARY KEY ("db_id")
);

ALTER TABLE "shoppingcart"."event_tracking"
    OWNER to postgres;

-- cart table
CREATE TABLE "shoppingcart"."cart"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "version" bigint NOT NULL,
    "event_type" CHARACTER VARYING(255) COLLATE pg_catalog."default"  NOT NULL,
    "event" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "cart_pkey" PRIMARY KEY ("db_id")
    ,CONSTRAINT "cart_version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."cart"
    OWNER to postgres;

-- coupon table
CREATE TABLE "shoppingcart"."coupon"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "version" bigint NOT NULL,
    "event_type" CHARACTER VARYING(255) COLLATE pg_catalog."default"  NOT NULL,
    "event" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "coupon_pkey" PRIMARY KEY ("db_id")
    ,CONSTRAINT "coupon_version_uq" UNIQUE ("id", "version")
);

ALTER TABLE "shoppingcart"."coupon"
    OWNER to postgres;

 -- shoppingcart Schema for view models
CREATE SCHEMA "shoppingcart_views"
    AUTHORIZATION postgres;

-- item table
CREATE TABLE "shoppingcart_views"."item"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "content" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "v_item_pkey" PRIMARY KEY ("db_id")
    ,CONSTRAINT "v_item_uq" UNIQUE ("id")
);

ALTER TABLE "shoppingcart_views"."item"
    OWNER to postgres;

-- item_type table
CREATE TABLE "shoppingcart_views"."item_type"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "content" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "v_item_type_pkey" PRIMARY KEY ("db_id")
    ,CONSTRAINT "v_item_type_uq" UNIQUE ("id")
);

ALTER TABLE "shoppingcart_views"."item_type"
    OWNER to postgres;

-- coupon table
CREATE TABLE "shoppingcart_views"."coupon"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "content" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "v_coupon_pkey" PRIMARY KEY ("db_id")
    ,CONSTRAINT "v_coupon_uq" UNIQUE ("id")
);

ALTER TABLE "shoppingcart_views"."coupon"
    OWNER to postgres;