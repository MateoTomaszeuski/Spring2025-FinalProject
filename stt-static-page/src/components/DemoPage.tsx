import React from 'react';

export class DemoPage extends React.Component<Record<string, never>, {display: DisplayOptions}> {
  constructor() {
    super({});
    this.state = {
      display: "Pomodoro"
    }
  }
  render() {
    return(
      <div>
        <h1>Demo</h1>
        <PomodoroDisplay />
      </div>
    )
  }
}

class PomodoroDisplay extends React.Component {
  render() {
    return(
      <div>
        <p>Timer</p>
      </div>
    )
  }
}

type DisplayOptions = 
  | "Pomodoro";
