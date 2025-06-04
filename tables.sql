--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-06-04 20:35:32

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 4882 (class 0 OID 0)
-- Dependencies: 4
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 221 (class 1259 OID 16990)
-- Name: address; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.address (
    id integer NOT NULL,
    addresstype character varying(20),
    unitno character varying(50),
    addline1 character varying(200),
    addline2 character varying(200),
    landmark character varying(50),
    pincode integer,
    "areaOfLocality" character varying(50),
    city character varying(25),
    state character varying(25),
    customerid integer
);


ALTER TABLE public.address OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 17002)
-- Name: address_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.address_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 2147483647
    CACHE 1;


ALTER SEQUENCE public.address_id_seq OWNER TO postgres;

--
-- TOC entry 4883 (class 0 OID 0)
-- Dependencies: 222
-- Name: address_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.address_id_seq OWNED BY public.address.id;


--
-- TOC entry 218 (class 1259 OID 16960)
-- Name: customer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customer (
    id integer NOT NULL,
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
    customertype smallint
);


ALTER TABLE public.customer OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16959)
-- Name: customer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customer_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.customer_id_seq OWNER TO postgres;

--
-- TOC entry 4884 (class 0 OID 0)
-- Dependencies: 217
-- Name: customer_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customer_id_seq OWNED BY public.customer.id;


--
-- TOC entry 225 (class 1259 OID 17020)
-- Name: customeraudit; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customeraudit (
    id integer NOT NULL,
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
    customertype smallint
);


ALTER TABLE public.customeraudit OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 17036)
-- Name: customeraudit_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customeraudit_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 2147483647
    CACHE 1;


ALTER SEQUENCE public.customeraudit_id_seq OWNER TO postgres;

--
-- TOC entry 4885 (class 0 OID 0)
-- Dependencies: 226
-- Name: customeraudit_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customeraudit_id_seq OWNED BY public.customeraudit.id;


--
-- TOC entry 223 (class 1259 OID 17013)
-- Name: customermapping; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customermapping (
    id integer NOT NULL,
    customerid integer,
    externalcustomerid character varying(100)
);


ALTER TABLE public.customermapping OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 17018)
-- Name: customermapping_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customermapping_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    MAXVALUE 2147483647
    CACHE 1;


ALTER SEQUENCE public.customermapping_id_seq OWNER TO postgres;

--
-- TOC entry 4886 (class 0 OID 0)
-- Dependencies: 224
-- Name: customermapping_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customermapping_id_seq OWNED BY public.customermapping.id;


--
-- TOC entry 220 (class 1259 OID 16978)
-- Name: logs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.logs (
    id integer NOT NULL,
    level character varying(50) NOT NULL,
    message text NOT NULL,
    correlationid uuid,
    loggedat timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.logs OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16977)
-- Name: logs_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.logs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.logs_id_seq OWNER TO postgres;

--
-- TOC entry 4887 (class 0 OID 0)
-- Dependencies: 219
-- Name: logs_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.logs_id_seq OWNED BY public.logs.id;


--
-- TOC entry 4718 (class 2604 OID 17003)
-- Name: address id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.address ALTER COLUMN id SET DEFAULT nextval('public.address_id_seq'::regclass);


--
-- TOC entry 4715 (class 2604 OID 16963)
-- Name: customer id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customer ALTER COLUMN id SET DEFAULT nextval('public.customer_id_seq'::regclass);


--
-- TOC entry 4720 (class 2604 OID 17037)
-- Name: customeraudit id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customeraudit ALTER COLUMN id SET DEFAULT nextval('public.customeraudit_id_seq'::regclass);


--
-- TOC entry 4719 (class 2604 OID 17019)
-- Name: customermapping id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customermapping ALTER COLUMN id SET DEFAULT nextval('public.customermapping_id_seq'::regclass);


--
-- TOC entry 4716 (class 2604 OID 16981)
-- Name: logs id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs ALTER COLUMN id SET DEFAULT nextval('public.logs_id_seq'::regclass);


--
-- TOC entry 4728 (class 2606 OID 17017)
-- Name: customermapping customermapping_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customermapping
    ADD CONSTRAINT customermapping_pkey PRIMARY KEY (id);


--
-- TOC entry 4730 (class 2606 OID 17029)
-- Name: customeraudit customerrequestlog_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customeraudit
    ADD CONSTRAINT customerrequestlog_pkey PRIMARY KEY (id);


--
-- TOC entry 4724 (class 2606 OID 16986)
-- Name: logs logs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_pkey PRIMARY KEY (id);


--
-- TOC entry 4726 (class 2606 OID 16996)
-- Name: address pk_address_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.address
    ADD CONSTRAINT pk_address_id PRIMARY KEY (id);


--
-- TOC entry 4722 (class 2606 OID 16967)
-- Name: customer pk_customer_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customer
    ADD CONSTRAINT pk_customer_id PRIMARY KEY (id);


--
-- TOC entry 4731 (class 2606 OID 16997)
-- Name: address fk_customer_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.address
    ADD CONSTRAINT fk_customer_id FOREIGN KEY (customerid) REFERENCES public.customer(id);


-- Completed on 2025-06-04 20:35:32

--
-- PostgreSQL database dump complete
--

