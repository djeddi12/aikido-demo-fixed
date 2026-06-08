const makeApiCall = async () => {
    const companyJwtToken = ""
    await fetch('https://example.com/some/other/endpoint', { mehod: 'GET', headers: { 'Authorization': `Bearer ${companyJwtToken}` }})
}