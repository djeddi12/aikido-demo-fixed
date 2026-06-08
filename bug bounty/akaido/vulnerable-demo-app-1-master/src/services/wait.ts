import fetch from 'node-fetch';

const ALLOWED_HOSTS = ['localhost', '127.0.0.1'];

export const waitFor200 = (url: string | URL) =>
  new Promise((resolve, reject) => {
    try {
      const parsedUrl = new URL(url.toString());
      if (!ALLOWED_HOSTS.includes(parsedUrl.hostname)) {
        throw new Error('Host not allowed');
      }
      const interval = setInterval(async () => {
        try {
          console.log('waiting for', parsedUrl.toString());
          const { status } = await fetch('http://127.0.0.1');
          if (status === 200) {
            clearInterval(interval);
            resolve(status);
          }
        } catch (err) {
          // ignore as we are not connected yet
        }
      }, 1000);
    } catch (e) {
      reject(e);
    }
  });
