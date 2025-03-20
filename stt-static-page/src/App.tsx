import React from 'react'
import './App.css'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { MissingPage } from './components/MissingPage'
import { MainPage } from './components/MainPage'

class App extends React.Component {
  render() {
    return (
      <div id='app'>
        <BrowserRouter>
          <Header />
          <div id='page'>
            <Routes>
              <Route path='/' element={<MainPage />} />
              <Route path='*' element={<MissingPage />} />
            </Routes>
          </div>
        </BrowserRouter>
      </div>
    )
  }
}

class Header extends React.Component {
  render() {
    return (
      <header>
        <h1>Consilium</h1>
      </header>
    )
  }
}

export default App
