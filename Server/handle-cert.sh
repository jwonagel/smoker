

sudo rm -r -f cert
mkdir cert
sudo cp -L -r /etc/letsencrypt/live/roadchiefs.ddns.net/* ./cert/ 
cd cert
sudo openssl pkcs12 -export -out roadchiefs.ddns.net.pfx -inkey privkey.pem -in fullchain.pem -passout pass:6d1JEkjE6aUOZNHs

