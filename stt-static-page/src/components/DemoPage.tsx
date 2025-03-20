import React from 'react';
import { PomodoroDisplay } from './PomodoroDisplay';

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

type DisplayOptions = 
  | "Pomodoro";
