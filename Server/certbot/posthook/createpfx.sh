#!/bin/sh
openssl pkcs12 -export -out /etc/letsencrypt/live/roadchiefs.ddns.net/certificate.pfx -inkey /etc/letsencrypt/live/roadchiefs.ddns.net/privkey.pem -in /etc/letsencrypt/live/roadchiefs.ddns.net/cert.pem -passin pass:$pass -passout pass:$pass
