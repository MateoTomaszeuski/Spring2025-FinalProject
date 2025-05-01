import React from 'react';

export class ValidatePage extends React.Component {
  state = {
    email: '',
    token: ''
  }

  componentDidMount(): void {
    const urlParams = new URLSearchParams(window.location.search);
    const email = urlParams.get('email');
    const token = urlParams.get('token');

    if (email || token) {
      this.setState({ email: email || '', token: token || '' });

      // Remove query parameters from the URL
      const newUrl = window.location.origin + window.location.pathname;
      window.history.replaceState({}, document.title, newUrl);
    }
  }

  render() {
    const url = `https://consilium-api-cpgdcqaxepbyc2gj.westus3-01.azurewebsites.net/account/validate?email=${this.state.email}&token=${this.state.token}`;
    return (
      <div id='validate-page'>
        <h2>Validate Page</h2>
        {this.state.email !== '' && (
          <>
            <p>For email: {this.state.email}</p>
            <a className='download-button' href={url}>Validate</a>
          </>
        )}
        <p><i>Why do we do this?</i><br />
          Some email providers check links before showing the email to you. In some cases,
          this means that the validation email never comes to you and you get automatically
          validated; which seems great, but it means anyone that knows your email has access
          to your account.
        </p>

      </div>
    );
  }
}
