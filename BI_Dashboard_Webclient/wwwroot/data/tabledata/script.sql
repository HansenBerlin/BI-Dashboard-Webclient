create table buy_score
(
    id                       int    not null
        primary key,
    score                    double null,
    sc_livingspace_sqm_price int    null,
    sc_lotarea_sqm_price     double null,
    sc_purchase_price        double null,
    sc_pricetrend            double null,
    sc_condition             double null,
    sc_firing_type           double null
);

create index idx_id_buy_score
    on buy_score (id);

create table immoscout_buy
(
    id                     int        not null
        primary key,
    pricetrend             double     null,
    pricetrendbuy          double     null,
    lotArea                int        null,
    yearConstructed        int        null,
    firingTypes            text       null,
    livingSpace            int        null,
    geoplz                 varchar(5) null,
    `condition`            text       null,
    noRooms                double     null,
    immotype               text       null,
    purchasePrice          double     null,
    telekomUploadSpeed     text       null,
    telekomDownloadSpeed   text       null,
    lat                    double     null,
    lon                    double     null,
    pricepersqmlivingspace double     null,
    pricepersqmlotarea     double     null
);

create index idx_id_immoscout_buy
    on immoscout_buy (id);

create table immoscout_rent
(
    id                 int        not null
        primary key,
    pricetrend         double     null,
    totalRent          int        null,
    serviceCharge      int        null,
    baseRent           int        null,
    livingSpace        int        null,
    noRooms            int        null,
    noParkSpaces       int        null,
    balcony            int        null,
    hasKitchen         int        null,
    cellar             int        null,
    `condition`        text       null,
    interiorQual       text       null,
    lift               int        null,
    typeOfFlat         text       null,
    telekomUploadSpeed double     null,
    floor              int        null,
    garden             int        null,
    heatingType        text       null,
    firingTypes        text       null,
    yearConstructed    int        null,
    lon                double     null,
    lat                double     null,
    pricepersqmbase    double     null,
    pricepersqmservice double     null,
    pricepersqmtotal   double     null,
    plz                varchar(5) not null,
    agskey             varchar(5) null
);

create index idx_id_immoscout_rent
    on immoscout_rent (id);

create table landkreise
(
    agskey               varchar(5) not null
        primary key,
    buildingPermits      double     null,
    landPrices           double     null,
    householdIncome2019  double     null,
    consumerInsolvencies double     null
);

create index idx_id_landkreise
    on landkreise (agskey);

create table plz_in_ags
(
    plz    varchar(5) not null
        primary key,
    agskey varchar(5) null
);

create index idx_id_plz_in_ags
    on plz_in_ags (plz);

create table rent_aggregates
(
    id          int    not null
        primary key,
    score       double null,
    rentMargin  double null,
    priceSqm    double null,
    heating     double null,
    `condition` double null,
    lat         double null,
    lon         double null
);

create index idx_id_rent_aggregates
    on rent_aggregates (id);

create table rent_aggregates_info
(
    id                 int    not null
        primary key,
    score              double null,
    rentMarginScore    double null,
    priceSmScore       double null,
    heatingScore       double null,
    conditionScore     double null,
    livingSpace        double null,
    `condition`        text   null,
    interiorQual       text   null,
    typeOfFlat         text   null,
    heatingType        text   null,
    yearConstructed    int    null,
    pricepersqmbase    double null,
    pricepersqmservice double null
);

create index idx_id_rent_aggregates_infp
    on rent_aggregates_info (id);


