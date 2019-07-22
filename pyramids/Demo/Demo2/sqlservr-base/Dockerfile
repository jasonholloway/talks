FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu

RUN apt-get update \
 && apt-get install -y --no-install-recommends \
    libprotobuf-dev libprotobuf-c0-dev protobuf-c-compiler protobuf-compiler python-protobuf

RUN apt-get install -y --no-install-recommends \
    git build-essential pkg-config python-ipaddress libbsd-dev iproute2 \
		libcap-dev libnl-3-dev libnet-dev libaio-dev python3-future xmlto

RUN apt-get install -y wget psmisc vim

WORKDIR /criu

ENV CRIU_VERSION "3.12"
RUN wget -q https://github.com/xemul/criu/archive/v${CRIU_VERSION}.tar.gz \
 	&& tar xzf v${CRIU_VERSION}.tar.gz \
 	&& cd criu-${CRIU_VERSION}/ && make -j \
 	&& cp criu/criu /usr/local/bin/criu 

ENV ACCEPT_EULA=y
ENV MSSQL_SA_PASSWORD=Wibble123!
ENV PATH="$PATH:/opt/mssql/bin:/opt/mssql-tools/bin"

WORKDIR /root

COPY ./ ./

CMD [ "./start.sh" ]
