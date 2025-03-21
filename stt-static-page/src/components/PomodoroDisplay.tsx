import React from 'react';

export class PomodoroDisplay extends React.Component<Record<string, never>,
  { currentTimer: number, active: boolean, currentAction: Actions, breakTime: number, workTime: number }> {
  canvasRef: React.RefObject<HTMLCanvasElement | null>;
  ctx: CanvasRenderingContext2D | null | undefined;
  timerInterval: NodeJS.Timeout | null;

  constructor() {
    super({});
    this.state = {
      currentTimer: 12,
      breakTime: 10,
      workTime: 12,
      active: false,
      currentAction: "Working"
    }
    this.canvasRef = React.createRef();
    this.timerInterval = null;
  }

  componentDidMount(): void {
    const canvas = this.canvasRef?.current;
    this.ctx = canvas?.getContext('2d');
    this.renderFrame();
  }

  componentDidUpdate() {
    this.renderFrame();
  }

  renderFrame() {
    if (this.ctx !== null && this.ctx !== undefined) {
      this.ctx.clearRect(0, 0, 400, 400);

      // Set background color here
      this.ctx.fillStyle = "#FAF7F2";
      this.ctx.fillRect(0, 0, 400, 400);

      // Background circle
      this.ctx.fillStyle = "white";
      this.ctx.beginPath();
      this.ctx.arc(200, 200, 165, 0, 2 * Math.PI);
      this.ctx.fill();

      // Set fill color here
      this.ctx.fillStyle = this.state.currentAction === "Working" ? "rgb(163, 193, 176, 0.6)" : "White";
      // Draw circle
      this.ctx.beginPath();
      this.ctx.moveTo(200, 200);
      this.ctx.lineTo(200, 50);
      const percentage: number = this.state.currentAction === "Working" ? (this.state.currentTimer / this.state.workTime) : (this.state.currentTimer / this.state.breakTime);
      this.ctx.arc(200, 200, 150, (3 * Math.PI) / 2, this.percentageToRadians(percentage * 100));
      this.ctx.moveTo(200, 200);
      this.ctx.fill();


      // Now to display the overlaid text
      // Set font, font size, color here
      this.ctx.font = "bold 48px Inter";
      this.ctx.strokeStyle = "#242424";
      this.ctx.fillStyle = "#242424";

      this.ctx.textAlign = "center";
      this.ctx.textBaseline = "middle";

      // Use either one, whether you want stroke or filled text
      this.ctx.strokeText(toDuration(this.state.currentTimer), 200, 200);
      this.ctx.fillText(toDuration(this.state.currentTimer), 200, 200);
    }
  }

  // thank you ChatGPT
  percentageToRadians = (percentage: number): number => {
    const startAngle = (3 * Math.PI) / 2; 
    const fullCircle = 2 * Math.PI;
    return startAngle + (fullCircle * (percentage / 100));
  }


  startTimer = () => {
    this.timerInterval = setInterval(() => {
      const newTime = this.state.currentTimer - 1;
      this.setState({ currentTimer: newTime });

      if (newTime <= 0) {
        switch (this.state.currentAction) {
          case "Working": {
            this.setState({ currentAction: "Break" });
            this.setState({ currentTimer: this.state.breakTime });
            window.alert("Stop working!");
            break;
          }
          case "Break": {
            this.setState({ currentAction: "Working" });
            this.setState({ currentTimer: this.state.workTime });
            window.alert("Start working!");
            break;
          }
        }
      }
    }, 1000);
  }

  stopTimer = () => {
    if (this.timerInterval !== null) {
      clearInterval(this.timerInterval);
    }
  }

  resetTimer = () => {
    this.setState({ currentTimer: this.state.workTime, currentAction: "Working" });
  }

  setBreakTime = (n: number) => {
    this.setState({ breakTime: n });
  }

  setWorkTime = (n: number) => {
    this.setState({ workTime: n });
  }

  render() {
    return (
      <div id="pomodoro-display">
        <canvas
          id='timer-display'
          ref={this.canvasRef}
          width={400}
          height={400}
          style={{ borderRadius: '3px' }}
        />
        {/* <p>Current state: {this.state.currentAction}</p>
        <p>Time Remaining: {toDuration(this.state.currentTimer)}</p> */}

        <div id="pomodoro-controls">
          <button onClick={this.startTimer}>Start</button>
          <button onClick={this.stopTimer}>Stop</button>
          <button onClick={this.resetTimer}>Reset</button>
        </div>

        <div id="pomodoro-settings">

          <div>
            <label htmlFor="work">Set work time</label>
            <select name="work" onChange={(e) => this.setWorkTime(parseInt(e.target.value))}>
              <option value="600">10</option>
              <option value="900">15</option>
              <option value="1200">20</option>
              <option value="1800">30</option>
            </select>
          </div>

          <div>

            <br />
            <label htmlFor="break">Set break time</label>
            <select name="work" onChange={(e) => this.setBreakTime(parseInt(e.target.value))} defaultValue={300}>
              <option value="60">1</option>
              <option value="180">3</option>
              <option value="300">5</option>
              <option value="600">10</option>
            </select>
          </div>
        </div>
      </div>
    );
  }
}

type Actions = "Working" | "Break";

function toDuration(seconds: number): string {
  const minutes = Math.floor((seconds % 3600) / 60);
  seconds = seconds % 60;

  return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
}
