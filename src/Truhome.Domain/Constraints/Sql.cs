namespace Truhome.Domain.Constraints;

internal static class Sql
{
    internal const string SCHEMA_SCRIPT = @"-- Ensure the public schema exists
CREATE SCHEMA IF NOT EXISTS public;

-- Optional: Comment is idempotent, safe to repeat
COMMENT ON SCHEMA public IS 'standard public schema';

-- Customer table
CREATE SEQUENCE IF NOT EXISTS public.customer_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;

CREATE TABLE IF NOT EXISTS public.customer (
    id integer NOT NULL DEFAULT nextval('public.customer_id_seq'),
    firstname character varying(100),
    middlename character varying(100),
    lastname character varying(100),
    dateofbirth date,
    mobilenumber bigint,
    drivinglicensenumber character varying(50),
    passportnumber character varying(50),
    pannumber character varying(20),
    aadharnumber character varying(20),
    ckycid character varying(20),
    voterid character varying(50),
    fatherfirstname character varying(100),
    fathermiddlename character varying(100),
    fatherlastname character varying(100),
    spousefirstname character varying(100),
    spousemiddlename character varying(100),
    spouselastname character varying(100),
    mothermaidenname character varying(100),
    emailid character varying(100),
    gender character varying(10),
    alternatemobilenumber bigint,
    companyname character varying(100),
    cin character varying(25),
    sourcesystem character varying(25),
    customertype smallint,
    PRIMARY KEY (id)
);

-- Address table
CREATE SEQUENCE IF NOT EXISTS public.address_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE MAXVALUE 2147483647 CACHE 1;

CREATE TABLE IF NOT EXISTS public.address (
    id integer NOT NULL DEFAULT nextval('public.address_id_seq'),
    addresstype character varying(20),
    unitno character varying(50),
    addline1 character varying(200),
    addline2 character varying(200),
    landmark character varying(50),
    pincode integer,
    ""areaOfLocality"" character varying(50),
    city character varying(25),
    state character varying(25),
    customerid integer,
    PRIMARY KEY (id),
    CONSTRAINT fk_customer_id FOREIGN KEY (customerid) REFERENCES public.customer(id)
);

-- CustomerMapping table
CREATE SEQUENCE IF NOT EXISTS public.customermapping_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE MAXVALUE 2147483647 CACHE 1;

CREATE TABLE IF NOT EXISTS public.customermapping (
    id integer NOT NULL DEFAULT nextval('public.customermapping_id_seq'),
    customerid integer,
    externalcustomerid character varying(100),
    PRIMARY KEY (id)
);

-- CustomerAudit table
CREATE SEQUENCE IF NOT EXISTS public.customeraudit_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE MAXVALUE 2147483647 CACHE 1;

CREATE TABLE IF NOT EXISTS public.customeraudit (
    id integer NOT NULL DEFAULT nextval('public.customeraudit_id_seq'),
    firstname character varying(100),
    middlename character varying(100),
    lastname character varying(100),
    dateofbirth date,
    mobilenumber bigint,
    drivinglicensenumber character varying(50),
    passportnumber character varying(50),
    pannumber character varying(20),
    aadharnumber character varying(20),
    ckycid character varying(20),
    voterid character varying(50),
    fatherfirstname character varying(100),
    fathermiddlename character varying(100),
    fatherlastname character varying(100),
    spousefirstname character varying(100),
    spousemiddlename character varying(100),
    spouselastname character varying(100),
    mothermaidenname character varying(100),
    emailid character varying(100),
    gender character varying(10),
    alternatemobilenumber bigint,
    companyname character varying(100),
    cin character varying(25),
    sourcesystem character varying(25),
    customerid integer,
    correlationid character varying(50),
    customertype smallint,
    PRIMARY KEY (id)
);

-- Logs table
CREATE SEQUENCE IF NOT EXISTS public.logs_id_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE CACHE 1;

CREATE TABLE IF NOT EXISTS public.logs (
    id integer NOT NULL DEFAULT nextval('public.logs_id_seq'),
    level character varying(50) NOT NULL,
    message text NOT NULL,
    correlationid uuid,
    loggedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    PRIMARY KEY (id)
);

";
}
