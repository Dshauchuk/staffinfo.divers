--
-- PostgreSQL database dump
--

-- Dumped from database version 11.7 (Ubuntu 11.7-2.pgdg16.04+1)
-- Dumped by pg_dump version 12.2

-- Started on 2020-03-27 12:25:19

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 196 (class 1259 OID 16059659)
-- Name: divers_id_seq; Type: SEQUENCE; Schema: public; Owner: lkugpsombylxoq
--

CREATE SEQUENCE public.divers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.divers_id_seq OWNER TO lkugpsombylxoq;

SET default_tablespace = '';

--
-- TOC entry 197 (class 1259 OID 16059661)
-- Name: divers; Type: TABLE; Schema: public; Owner: lkugpsombylxoq
--

CREATE TABLE public.divers (
    diver_id bigint DEFAULT nextval('public.divers_id_seq'::regclass) NOT NULL,
    last_name character varying(150) NOT NULL,
    first_name character varying(150) NOT NULL,
    middle_name character varying(150),
    photo_url text,
    birth_date date,
    rescue_station_id bigint,
    medical_examination_date date,
    address character varying(500),
    qualification integer,
    personal_book_number character varying(10) NOT NULL,
    personal_book_issue_date date NOT NULL,
    personal_book_protocol_number character varying(10) NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updated_at timestamp with time zone
);


ALTER TABLE public.divers OWNER TO lkugpsombylxoq;

--
-- TOC entry 216 (class 1255 OID 16059668)
-- Name: sf_add_diver(character varying, character varying, character varying, character varying, date, bigint, date, character varying, integer, character varying, date, character varying); Type: FUNCTION; Schema: public; Owner: lkugpsombylxoq
--

CREATE FUNCTION public.sf_add_diver(p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_photo_url character varying, p_birth_date date, p_station_id bigint, p_medical_examination_date date, p_address character varying, p_qualification integer, p_personal_book_number character varying, p_personal_book_issue_date date, p_personal_book_protocol_number character varying) RETURNS SETOF public.divers
    LANGUAGE plpgsql
    AS $$
begin
	return query
		insert into public.divers(last_name, first_name, middle_name, photo_url, birth_date, rescue_station_id, medical_examination_date, address, qualification, personal_book_number, personal_book_issue_date, personal_book_protocol_number)
		values(p_last_name, p_first_name, p_middle_name, p_photo_url, p_birth_date, p_station_id, p_medical_examination_date, p_address, p_qualification, p_personal_book_number, p_personal_book_issue_date, p_personal_book_protocol_number)
		returning *;			   
end;
$$;


ALTER FUNCTION public.sf_add_diver(p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_photo_url character varying, p_birth_date date, p_station_id bigint, p_medical_examination_date date, p_address character varying, p_qualification integer, p_personal_book_number character varying, p_personal_book_issue_date date, p_personal_book_protocol_number character varying) OWNER TO lkugpsombylxoq;

--
-- TOC entry 198 (class 1259 OID 16059669)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: lkugpsombylxoq
--

CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_id_seq OWNER TO lkugpsombylxoq;

--
-- TOC entry 199 (class 1259 OID 16059671)
-- Name: users; Type: TABLE; Schema: public; Owner: lkugpsombylxoq
--

CREATE TABLE public.users (
    user_id bigint DEFAULT nextval('public.users_id_seq'::regclass) NOT NULL,
    last_name character varying(150),
    first_name character varying(150),
    middle_name character varying(150),
    login character varying(50) NOT NULL,
    pwd_hash character varying(500) NOT NULL,
    need_to_change_pwd boolean DEFAULT false NOT NULL,
    refresh_token character varying(500),
    token_refresh_timestamp timestamp with time zone,
    role character varying(50) NOT NULL,
    registration_timestamp timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.users OWNER TO lkugpsombylxoq;

--
-- TOC entry 219 (class 1255 OID 16059680)
-- Name: sf_add_user(character varying, character varying, character varying, character varying, character varying, boolean, character varying); Type: FUNCTION; Schema: public; Owner: lkugpsombylxoq
--

CREATE FUNCTION public.sf_add_user(p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_login character varying, p_pwd_hash character varying, p_need_to_change_pwd boolean, role character varying) RETURNS SETOF public.users
    LANGUAGE plpgsql
    AS $$
begin
	return query
		insert into public.users(last_name, first_name, middle_name, login, pwd_hash, need_to_change_pwd, role, registration_timestamp)
		values(p_last_name, p_first_name, p_middle_name, p_login, p_pwd_hash, p_need_to_change, p_role, CURRENT_TIMESTAMP)
		returning *;			   
end;
$$;


ALTER FUNCTION public.sf_add_user(p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_login character varying, p_pwd_hash character varying, p_need_to_change_pwd boolean, role character varying) OWNER TO lkugpsombylxoq;

--
-- TOC entry 210 (class 1255 OID 16688264)
-- Name: sf_get_divers(integer, date, date, integer, integer, text); Type: FUNCTION; Schema: public; Owner: lkugpsombylxoq
--

CREATE FUNCTION public.sf_get_divers(p_station_id integer, p_med_exam_start_date date, p_med_exam_end_date date, p_min_qualif integer, p_max_qualif integer, p_name_query text) RETURNS TABLE(diver_id bigint, last_name character varying, first_name character varying, middle_name character varying, photo_url character varying, birth_date date, rescue_station_id bigint, medical_examination_date date, address character varying, qualification integer, personal_book_number character varying, personal_book_issue_date date, personal_book_protocol_number character varying, created_at timestamp with time zone, updated_at timestamp with time zone)
    LANGUAGE plpgsql
    AS $_$

declare 
	sql_text text; 
begin
	sql_text := 'select * from divers where 1=1 ';
	
	if(p_station_id is not null)
		then
			sql_text := sql_text || ' and rescue_station_id = $1 ';
	end if;
	
	if(p_med_exam_start_date is not null)
		then
			sql_text := sql_text || ' and medical_examination_date >= $2 ';
	end if;	  
	
	if(p_med_exam_end_date is not null)
		then
			sql_text := sql_text || ' and medical_examination_date <= $3 ';
	end if;	   
	
	if(p_min_qualif is not null)
		then
			sql_text := sql_text || ' and qualification >= $4 ';
	end if;	  
	
	if(p_max_qualif is not null)
		then
			sql_text := sql_text || ' and qualification <= $5 ';
	end if;	
	
	if(p_name_query is not null)
		then
			sql_text := sql_text || ' and lower(concat(last_name, '' '', first_name, '' '',middle_name)) like ''%'' || lower($6) || ''%'' ';
	end if;
	
	return query
	execute sql_text
	using p_station_id, p_med_exam_start_date, p_med_exam_end_date, p_min_qualif, p_max_qualif, p_name_query;
end;

$_$;


ALTER FUNCTION public.sf_get_divers(p_station_id integer, p_med_exam_start_date date, p_med_exam_end_date date, p_min_qualif integer, p_max_qualif integer, p_name_query text) OWNER TO lkugpsombylxoq;

--
-- TOC entry 212 (class 1255 OID 16059681)
-- Name: sf_update_diver(bigint, character varying, character varying, character varying, character varying, date, bigint, date, character varying, integer, character varying, date, character varying); Type: FUNCTION; Schema: public; Owner: lkugpsombylxoq
--

CREATE FUNCTION public.sf_update_diver(p_diver_id bigint, p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_photo_url character varying, p_birth_date date, p_station_id bigint, p_medical_examination_date date, p_address character varying, p_qualification integer, p_personal_book_number character varying, p_personal_book_issue_date date, p_personal_book_protocol_number character varying) RETURNS SETOF public.divers
    LANGUAGE plpgsql
    AS $$
begin
	update public.divers
	set last_name = p_last_name,
		first_name = p_first_name,
		middle_name = p_middle_name,
		photo_url = p_photo_url,
		birth_date = p_birth_date,
		rescue_station_id = p_station_id,
		medical_examination_date = p_medical_examination_date,
		address = p_address,
		qualification = p_qualification,
		personal_book_number = p_personal_book_number,
		personal_book_issue_date = p_personal_book_issue_date,
		personal_book_protocol_number = p_personal_book_protocol_number
	where diver_id = p_diver_id;
	
	return query
	select *
	from public.divers
	where diver_id = p_diver_id;
end;
$$;


ALTER FUNCTION public.sf_update_diver(p_diver_id bigint, p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_photo_url character varying, p_birth_date date, p_station_id bigint, p_medical_examination_date date, p_address character varying, p_qualification integer, p_personal_book_number character varying, p_personal_book_issue_date date, p_personal_book_protocol_number character varying) OWNER TO lkugpsombylxoq;

--
-- TOC entry 207 (class 1255 OID 16059682)
-- Name: sf_update_user(bigint, character varying, character varying, character varying, character varying, character varying, boolean, character varying); Type: FUNCTION; Schema: public; Owner: lkugpsombylxoq
--

CREATE FUNCTION public.sf_update_user(p_user_id bigint, p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_login character varying, p_pwd_hash character varying, p_need_to_change_pwd boolean, role character varying) RETURNS SETOF public.users
    LANGUAGE plpgsql
    AS $$
begin
	
	update users
	set last_name = p_last_name,
		first_name = p_first_name,
		middle_name = p_middle_name,
		login = p_login,
		pwd_hash = p_pwd_hash,
		need_to_change_pwd = p_need_to_change_pwd
	where user_id = p_user_id;
	
	return query
	select * 
	from users
	where user_id = p_user_id;
	
end;
$$;


ALTER FUNCTION public.sf_update_user(p_user_id bigint, p_last_name character varying, p_first_name character varying, p_middle_name character varying, p_login character varying, p_pwd_hash character varying, p_need_to_change_pwd boolean, role character varying) OWNER TO lkugpsombylxoq;

--
-- TOC entry 200 (class 1259 OID 16059683)
-- Name: diving_hours; Type: TABLE; Schema: public; Owner: lkugpsombylxoq
--

CREATE TABLE public.diving_hours (
    diver_id bigint NOT NULL,
    year integer NOT NULL,
    working_minutes bigint DEFAULT 0 NOT NULL
);


ALTER TABLE public.diving_hours OWNER TO lkugpsombylxoq;

--
-- TOC entry 201 (class 1259 OID 16059687)
-- Name: rescue_stations_id_seq; Type: SEQUENCE; Schema: public; Owner: lkugpsombylxoq
--

CREATE SEQUENCE public.rescue_stations_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.rescue_stations_id_seq OWNER TO lkugpsombylxoq;

--
-- TOC entry 202 (class 1259 OID 16059689)
-- Name: rescue_stations; Type: TABLE; Schema: public; Owner: lkugpsombylxoq
--

CREATE TABLE public.rescue_stations (
    station_id bigint DEFAULT nextval('public.rescue_stations_id_seq'::regclass) NOT NULL,
    station_name character varying(300) NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updated_at timestamp with time zone
);


ALTER TABLE public.rescue_stations OWNER TO lkugpsombylxoq;

--
-- TOC entry 3866 (class 0 OID 16059661)
-- Dependencies: 197
-- Data for Name: divers; Type: TABLE DATA; Schema: public; Owner: lkugpsombylxoq
--

COPY public.divers (diver_id, last_name, first_name, middle_name, photo_url, birth_date, rescue_station_id, medical_examination_date, address, qualification, personal_book_number, personal_book_issue_date, personal_book_protocol_number, created_at, updated_at) FROM stdin;
37	Шевчук	Павел	Анатольевич	\N	\N	\N	\N	г.Гомель, м-н Энергетиков, 33/7	1	123123	2019-06-12	25	2019-12-16 16:35:35.042944+00	\N
38	Жигар	Дмитрий	qweeqeq	\N	1994-07-04	116	2020-01-31	Садовая 18	1	1	2020-01-31	1	2020-01-07 12:04:08.563308+00	\N
39	йцукен	йцукеен	йцуке	\N	1984-08-07	116	\N	Садовая 18	1	1	2019-11-12	1	2020-01-07 12:16:46.946107+00	\N
40	Шевчук	Павел	Анатольевич	\N	0001-01-01	\N	\N	г.Гомель, м-н Энергетиков, 33/7	1	123123	2019-06-12	25	2020-01-13 20:07:29.759398+00	\N
41	йцукен	йцукеен	йцуке	\N	1984-08-07	\N	\N	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:11:03.891775+00	\N
42	йцукен	йцукеен	йцуке	\N	1984-08-07	\N	\N	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:18:58.363885+00	\N
43	Авдосий	Йоан	Петрович	\N	1984-08-07	\N	\N	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:19:53.857921+00	\N
44	Авдосий	Йоан	Петрович	\N	1984-08-07	\N	2020-01-16	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:20:42.332828+00	\N
45	Авдосий1111	Йоан	Петрович	\N	1984-08-07	\N	2020-01-21	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:21:44.359949+00	\N
46	Авдосий1111	Йоан	Петрович	\N	1984-08-07	\N	2020-01-21	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:24:15.986308+00	\N
47	Авдосий1111	Йоан	Петрович	\N	1984-08-07	\N	2020-01-21	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:24:25.596149+00	\N
48	йцукен	йцукеен	йцуке	\N	1984-08-07	\N	\N	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:26:59.386758+00	\N
49	йцукен	йцукеен	йцуке	\N	1984-08-07	116	\N	Садовая 18	1	1	2019-11-12	1	2020-01-13 20:49:16.142787+00	\N
50	йцукен	йцукеен	йцуке	\N	1984-08-07	116	\N	Садовая 18	1	1	2019-11-12	1	2020-01-14 18:39:02.41669+00	\N
51	йцукен	йцукеен	йцуке	\N	1984-08-07	116	\N	Садовая 18	1	1	2019-11-12	1	2020-01-14 18:52:34.046182+00	\N
52	йцукен	йцукеен	йцуке	\N	1984-08-07	116	\N	Садовая 18	1	1	2019-11-12	1	2020-01-19 09:05:12.967747+00	\N
53	йцукен	йцукеен	йцуке	\N	1984-08-07	116	\N	Садовая 18	1	1	2019-11-12	1	2020-01-19 09:07:29.514591+00	\N
54	йцукен	йцукеен	йцуке	\N	1984-08-07	116	\N	Садовая 18	1	1	2019-11-12	1	2020-01-19 09:09:01.124186+00	\N
55	Авдосий1111	Йоан	Петрович	shot_012.jpg	1984-08-07	\N	2020-01-21	Садовая 18	1	1	2019-11-12	1	2020-01-26 16:24:14.216965+00	\N
56	Авдосий1111	Йоан	Петрович	shot_013.jpg	1984-08-07	116	2020-01-21	Садовая 18	1	1	2019-11-12	1	2020-01-26 16:25:59.400733+00	\N
57	Шевчук	Павел	Анатольевич	\N	0001-01-01	\N	\N	г.Гомель, м-н Энергетиков, 33/7	1	123123	2019-06-12	25	2020-01-29 20:10:23.545172+00	\N
58	Жигар	Дмитрий	qweeqeq	\N	1994-07-04	116	2020-01-31	Садовая 18	1	1	2020-01-31	1	2020-01-29 20:10:34.680109+00	\N
59	Шевчук	Павел	Анатольевич	\N	0001-01-01	\N	\N	г.Гомель, м-н Энергетиков, 33/7	1	123123	2019-06-12	25	2020-01-29 20:28:46.33377+00	\N
60	Шевчук	Павел	Анатольевич	\N	0001-01-01	\N	\N	г.Гомель, м-н Энергетиков, 33/7	1	123123	2019-06-12	25	2020-01-29 20:32:10.618326+00	\N
61	123	123	123	\N	2020-03-05	117	2020-03-16	123	2	1223	2020-03-20	123	2020-03-25 07:53:10.741109+00	\N
\.


--
-- TOC entry 3869 (class 0 OID 16059683)
-- Dependencies: 200
-- Data for Name: diving_hours; Type: TABLE DATA; Schema: public; Owner: lkugpsombylxoq
--

COPY public.diving_hours (diver_id, year, working_minutes) FROM stdin;
\.


--
-- TOC entry 3871 (class 0 OID 16059689)
-- Dependencies: 202
-- Data for Name: rescue_stations; Type: TABLE DATA; Schema: public; Owner: lkugpsombylxoq
--

COPY public.rescue_stations (station_id, station_name, created_at, updated_at) FROM stdin;
116	Гомельская городская	2019-12-06 15:32:05.99495+00	\N
117	Западная	2019-12-06 15:32:37.273726+00	\N
119	Ветковская	2019-12-06 15:33:22.25502+00	\N
120	Жлобинская	2019-12-06 15:33:38.323791+00	\N
118	Новобелицкая	2019-12-06 15:33:06.760272+00	2020-03-25 07:51:19.768739+00
\.


--
-- TOC entry 3868 (class 0 OID 16059671)
-- Dependencies: 199
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: lkugpsombylxoq
--

COPY public.users (user_id, last_name, first_name, middle_name, login, pwd_hash, need_to_change_pwd, refresh_token, token_refresh_timestamp, role, registration_timestamp) FROM stdin;
7	Shauchuk	Dzmitry	Paulavich	dshauchuk	B1B3773A05C0ED0176787A4F1574FF0075F7521E	f	28932ecd-9cda-4528-8d99-0ef585c7c5f8	2020-02-04 17:58:05.460392+00	admin	2019-10-06 12:50:19.109107+00
8	\N	\N	\N	admin	68AEB8C02944E4F501A967B26125EE9DACF07EDC	f	e93e847b-959b-4b67-8599-ad6d0d15ac71	2020-03-26 22:57:28.369007+00	user	2019-10-20 13:03:11.001932+00
\.


--
-- TOC entry 3879 (class 0 OID 0)
-- Dependencies: 196
-- Name: divers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lkugpsombylxoq
--

SELECT pg_catalog.setval('public.divers_id_seq', 61, true);


--
-- TOC entry 3880 (class 0 OID 0)
-- Dependencies: 201
-- Name: rescue_stations_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lkugpsombylxoq
--

SELECT pg_catalog.setval('public.rescue_stations_id_seq', 126, true);


--
-- TOC entry 3881 (class 0 OID 0)
-- Dependencies: 198
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: lkugpsombylxoq
--

SELECT pg_catalog.setval('public.users_id_seq', 8, true);


--
-- TOC entry 3733 (class 2606 OID 16059694)
-- Name: divers divers_pkey; Type: CONSTRAINT; Schema: public; Owner: lkugpsombylxoq
--

ALTER TABLE ONLY public.divers
    ADD CONSTRAINT divers_pkey PRIMARY KEY (diver_id);


--
-- TOC entry 3739 (class 2606 OID 16059696)
-- Name: diving_hours diving_hours_pkey; Type: CONSTRAINT; Schema: public; Owner: lkugpsombylxoq
--

ALTER TABLE ONLY public.diving_hours
    ADD CONSTRAINT diving_hours_pkey PRIMARY KEY (diver_id, year);


--
-- TOC entry 3741 (class 2606 OID 16059698)
-- Name: rescue_stations rescue_stations_pkey; Type: CONSTRAINT; Schema: public; Owner: lkugpsombylxoq
--

ALTER TABLE ONLY public.rescue_stations
    ADD CONSTRAINT rescue_stations_pkey PRIMARY KEY (station_id);


--
-- TOC entry 3735 (class 2606 OID 16593230)
-- Name: users un_for_login; Type: CONSTRAINT; Schema: public; Owner: lkugpsombylxoq
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT un_for_login UNIQUE (login);


--
-- TOC entry 3737 (class 2606 OID 16059700)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: lkugpsombylxoq
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);


--
-- TOC entry 3743 (class 2606 OID 16059701)
-- Name: diving_hours fk_divers; Type: FK CONSTRAINT; Schema: public; Owner: lkugpsombylxoq
--

ALTER TABLE ONLY public.diving_hours
    ADD CONSTRAINT fk_divers FOREIGN KEY (diver_id) REFERENCES public.divers(diver_id) ON DELETE CASCADE;


--
-- TOC entry 3742 (class 2606 OID 16059706)
-- Name: divers fk_station; Type: FK CONSTRAINT; Schema: public; Owner: lkugpsombylxoq
--

ALTER TABLE ONLY public.divers
    ADD CONSTRAINT fk_station FOREIGN KEY (rescue_station_id) REFERENCES public.rescue_stations(station_id) ON DELETE SET NULL;


--
-- TOC entry 3877 (class 0 OID 0)
-- Dependencies: 3
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: lkugpsombylxoq
--

REVOKE ALL ON SCHEMA public FROM postgres;
REVOKE ALL ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO lkugpsombylxoq;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- TOC entry 3878 (class 0 OID 0)
-- Dependencies: 617
-- Name: LANGUAGE plpgsql; Type: ACL; Schema: -; Owner: postgres
--

GRANT ALL ON LANGUAGE plpgsql TO lkugpsombylxoq;


-- Completed on 2020-03-27 12:25:30

--
-- PostgreSQL database dump complete
--

