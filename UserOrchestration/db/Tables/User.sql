CREATE TABLE IF NOT EXISTS public."User"
(
    "Id" integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "Username" text COLLATE pg_catalog."default" NOT NULL,
    "HashedPassword" text COLLATE pg_catalog."default" NOT NULL,
    "EmailAddress" text COLLATE pg_catalog."default" NOT NULL,
    "IsActive" boolean NOT NULL DEFAULT false,
    "IsPasswordChangeRequired" boolean NOT NULL DEFAULT true,
    "IsEmailVerified" boolean NOT NULL DEFAULT false,
    CONSTRAINT "User_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."User"
    OWNER to postgres;