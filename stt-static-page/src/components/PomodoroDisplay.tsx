import React from 'react';

export class PomodoroDisplay extends React.Component<Record<string, never>, 
  { currentTimer: number, active: boolean, currentAction: Actions, breakTime: number, workTime: number }> {
  constructor() {
    super({});
    this.state = {
      currentTimer: 600,
      breakTime: 300,
      workTime: 600,
      active: false, 
      currentAction: "Working"
    }
  }


  startTimer = () => {
    this.setState({active: true});
    const interval = setInterval(() => {
      if (!this.state.active) clearInterval(interval);
      const newTime = this.state.currentTimer - 1;
      this.setState({currentTimer: newTime});
      
      if (newTime <= 0) {
        switch (this.state.currentAction) {
          case "Working": {
            this.setState({currentAction: "Break"});
            this.setState({currentTimer: this.state.breakTime});
            window.alert("Stop working!");
            break;
          }
          case "Break": {
            this.setState({currentAction: "Working"});
            this.setState({currentTimer: this.state.workTime});
            window.alert("Start working!");
            break;
          }
        }
      }
    }, 1000);
  }

  stopTimer = () => {
    this.setState({active: false});
  }

  resetTimer = () => {
    this.setState({currentTimer: this.state.workTime});
  }

  setBreakTime = (n: number) => {
    this.setState({breakTime: n});
  }

  setWorkTime = (n: number) => {
    this.setState({workTime: n});
  }

  render() {
    return (
      <div>
        <p>Current state: {this.state.currentAction}</p>
        <p>Time Remaining: {toDuration(this.state.currentTimer)}</p>
        <button onClick={this.startTimer}>Start</button>
        <button onClick={this.stopTimer}>Stop</button>
        <button onClick={this.resetTimer}>Reset</button>

        <p>Set work time</p>
        <select onChange={(e) => this.setWorkTime(parseInt(e.target.value))}>
          <option value="600">10</option>
          <option value="900">15</option>
          <option value="1200">20</option>
          <option value="1800">30</option>
        </select>

        <p>Set break time</p>
        <select onChange={(e) => this.setBreakTime(parseInt(e.target.value))} defaultValue={300}>
          <option value="60">1</option>
          <option value="180">3</option>
          <option value="300">5</option>
          <option value="600">10</option>
        </select>
      </div>
    );
  }
}

type Actions = "Working" | "Break";

function toDuration(seconds: number): string {
  const minutes = Math.floor((seconds % 3600) / 60);
  seconds = seconds % 60;

  return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0') }`;
}
