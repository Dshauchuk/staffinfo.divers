
CREATE SEQUENCE public.divers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.divers_id_seq OWNER TO lkugpsombylxoq;

SET default_tablespace = '';

CREATE TABLE divers (
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

CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_id_seq OWNER TO lkugpsombylxoq;

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


CREATE FUNCTION public.sf_get_divers(p_station_id integer, p_med_exam_start_date date, p_med_exam_end_date date, p_min_qualif integer, p_max_qualif integer, p_name_query text) 
RETURNS TABLE(diver_id bigint, last_name character varying, first_name character varying, middle_name character varying, photo_url character varying, birth_date date, rescue_station_id bigint, 
    medical_examination_date date, address character varying, qualification integer, personal_book_number character varying, personal_book_issue_date date, personal_book_protocol_number character varying, 
	created_at timestamp with time zone, updated_at timestamp with time zone)
LANGUAGE plpgsql
AS $$

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
$$;

ALTER FUNCTION public.sf_get_divers(p_station_id integer, p_med_exam_start_date date, p_med_exam_end_date date, p_min_qualif integer, p_max_qualif integer, p_name_query text) OWNER TO lkugpsombylxoq;


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

CREATE TABLE public.diving_hours (
    diver_id bigint NOT NULL,
    year integer NOT NULL,
    working_minutes bigint DEFAULT 0 NOT NULL
);


ALTER TABLE public.diving_hours OWNER TO lkugpsombylxoq;

CREATE SEQUENCE public.rescue_stations_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.rescue_stations_id_seq OWNER TO lkugpsombylxoq;

CREATE TABLE public.rescue_stations (
    station_id bigint DEFAULT nextval('public.rescue_stations_id_seq'::regclass) NOT NULL,
    station_name character varying(300) NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    updated_at timestamp with time zone
);


ALTER TABLE public.rescue_stations OWNER TO lkugpsombylxoq;

SELECT pg_catalog.setval('public.divers_id_seq', 61, true);

SELECT pg_catalog.setval('public.rescue_stations_id_seq', 126, true);

SELECT pg_catalog.setval('public.users_id_seq', 8, true);

ALTER TABLE ONLY public.divers
    ADD CONSTRAINT divers_pkey PRIMARY KEY (diver_id);
    
ALTER TABLE ONLY public.diving_hours
    ADD CONSTRAINT diving_hours_pkey PRIMARY KEY (diver_id, year);
    
ALTER TABLE ONLY public.rescue_stations
    ADD CONSTRAINT rescue_stations_pkey PRIMARY KEY (station_id);

ALTER TABLE ONLY public.users
    ADD CONSTRAINT un_for_login UNIQUE (login);

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (user_id);

ALTER TABLE ONLY public.diving_hours
    ADD CONSTRAINT fk_divers FOREIGN KEY (diver_id) REFERENCES public.divers(diver_id) ON DELETE CASCADE;

ALTER TABLE ONLY public.divers
    ADD CONSTRAINT fk_station FOREIGN KEY (rescue_station_id) REFERENCES public.rescue_stations(station_id) ON DELETE SET NULL;

REVOKE ALL ON SCHEMA public FROM postgres;
REVOKE ALL ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO lkugpsombylxoq;
GRANT ALL ON SCHEMA public TO PUBLIC;

GRANT ALL ON LANGUAGE plpgsql TO lkugpsombylxoq;

