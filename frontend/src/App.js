import './App.css';

import https from 'https';

import React from 'react';

import Loading from "./Main/Loading/Loading"

import {
  Redirect
} from 'react-router';

class App extends React.Component {
    loaded = false;

    constructor(props) {
      super(props)

      this.state = {
        loadingMessage: "Connecting to Server...",
        redirectToLogin: false,
        redirectToLogout: false,
        redirectToApp: false
      }
    }

    componentDidMount() {
      console.log(this.props.connection)

      if (localStorage.getItem('account') !== null) {

        const data = localStorage.getItem('account')

        console.log(data)

        const options = {
          hostname: 'localhost',
          port: 5003,
          path: '/tokenlogin',
          method: 'GET'
        }


        const req = https.request(options, res => {
          console.log(`result:`)
          console.log(res)
          res.on('data', d => {
            var returnedDat = new TextDecoder().decode(d);
            var parsedDat = JSON.parse(returnedDat)
            console.log(parsedDat)

            if (parsedDat) {
              this.setState({
                redirectToApp: true
              })
            } else {
              this.loginWithRefreshToken()
            }
          })
        })

        req.setHeader('accessToken', data)

        req.on('error', error => {
          console.error(error)
        })

        req.write(data)
        req.end()

        this.setState({
          loadingMessage: "Getting Account Data..."
        })
      }
      else {
        this.loginWithRefreshToken()
      }


    }

    loginWithRefreshToken() {
      const refreshToken = localStorage.getItem('refreshToken')
      if (refreshToken == null) this.setState({
        redirectToLogin: true
      })

      const options2 = {
        hostname: 'localhost',
        port: 5003,
        path: '/generatewithrefreshtoken',
        method: 'GET'
      }

      const req2 = https.request(options2, res2 => {
        res2.on('data', data => {
          switch (res2.statusCode) {
            case 400:
              this.setState({
                redirectToLogin: true
              })
              break;
            case 404:
              this.setState({
                redirectToLogout: true
              })
            case 200:
              var returndata = new TextDecoder().decode(data);
              var parsedData = JSON.parse(returndata)
              console.log('parsed data:', parsedData)

              var content = JSON.parse(`${Buffer.from(parsedData.generatedToken.split(".")[1], 'base64')}`)
              sessionStorage.setItem('username', content.name)

              localStorage.setItem('account', parsedData.generatedToken);
              localStorage.setItem('refreshToken', parsedData.refreshToken.join(""));

              this.setState({
                redirectToApp: true
              })
              break;
          }
        })
      })

      req2.setHeader('token', refreshToken)

      req2.on('error', error2 => {
        console.error(error2)
      })

      req2.write(refreshToken)
      req2.end()
    }

    redirectToLogin() {
      this.setState({
        redirectToLogin: true
      })
    }

    render() {
      if (this.state.redirectToLogin) return ( <Redirect to = "/login" /> )
      if (this.state.redirectToLogout) return ( <Redirect to = "/logout" /> )
      if (this.state.redirectToApp) return ( <Redirect to = "/app" /> )

      if (!this.loaded) {
        return ( < Loading loadingMessage = {this.state.loadingMessage} />)
      }
      return ( <div> </div> );
      }
    }

    export default App;