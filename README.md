# Bishal Agro Seed

**To host on a same domain**

This guide outlines steps for developers to redirect public URLs to localhost with a different port.

**1. Modify Hosts File**
- Open hosts file as administrator (C:\Windows\System32\drivers\etc)
- Add the following line:
  ```
  127.11.11.11 bishalagroseed.com
  127.11.11.11 www.bishalagroseed.com
  ```
  Note: The IP address can be different; it doesn’t have to be 127.0.0.1.

**2. Use netsh Tool**
- Open command prompt as administrator.
- Execute the following command to forward requests from port 80 to port 5000:
  ```
  netsh interface portproxy add v4tov4 listenport=80 listenaddress=127.11.11.11 connectport=5000 connectaddress=127.0.0.1
  ```

**3. Cleanup**
- To remove redirection, delete the added line from the hosts file.
- Execute the following command to remove port forwarding:
  ```
  netsh interface portproxy delete v4tov4 listenport=80 listenaddress=127.11.11.11
  ```

**Note:** Ensure to clean up configurations after testing to avoid conflicts.