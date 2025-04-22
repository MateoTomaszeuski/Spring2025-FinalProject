import React, { JSX } from 'react';

export type CollapsingHeaderProps = {
  defaultOpen: boolean,
  headerText: string,
  idRef: string,
  children: React.ReactNode,
  headerType?: keyof JSX.IntrinsicElements
}

/**
 * Anything you enclose will be collapsed when you click on the header.
 * You can style like a class header, but I added the arrow that points right or down respectively.
 * Sample below:
 *
 * @example
 * <CollapsingHeader defaultOpen={true} headerText="lorem" idRef="navigateHere" headerType={'h1'}>
 *   <p>Inner paragraph text</p>
 * </CollapsingHeader>
 *
 * @param {boolean} defaultOpen - Is open by default when first loaded (for true)
 * @param {string} headerText - Text to be displayed in the header
 * @param {string} idRef - HTML ID to link to
 * @param {keyof JSX.IntrinsicElements} headerType (opt.) - Defaults to 'h2', but all header types can be passed in.
 */

export class CollapsingHeader extends React.Component<CollapsingHeaderProps, { isOpen: boolean; isHovered: boolean; isCopied: boolean; }> {
  constructor(props: CollapsingHeaderProps) {
    super(props);
    this.state = {
      isOpen: this.props.defaultOpen,
      isHovered: false,
      isCopied: false
    };
    this.headerRef = React.createRef();
  }
  headerRef: React.RefObject<HTMLElement>;

  componentDidMount() {
    // Only scroll if this header is the one targeted by the hash
    const currentHash = window.location.hash.replace('#', '');
    if (currentHash === this.props.idRef && this.headerRef.current) {
      this.headerRef.current.scrollIntoView({ behavior: 'smooth' });
    }
  }

  toggleOpen = () => {
    this.setState({ isOpen: !this.state.isOpen });
  };

  setHoverOn = () => { this.setState({ isHovered: true }); };
  setHoverOff = () => { this.setState({ isHovered: false }); };
  copyLink = () => {
    const fullLink = `${window.location.origin}${window.location.pathname}#${this.props.idRef}`;
    navigator.clipboard.writeText(fullLink);
    this.setState({ isCopied: true });
    setTimeout(() => {
      this.setState({ isCopied: false });
    }, 2000);
  };

  render() {
    const HeaderTag = this.props.headerType || 'h2'; // default if none provided

    // Don't mind the error here, everything works as intended anyways. 
    return (
      <>
        <HeaderTag onClick={this.toggleOpen}
          id={this.props.idRef}
          ref={this.headerRef}
          onMouseEnter={this.setHoverOn}
          onMouseLeave={this.setHoverOff}>
          {this.state.isOpen ? <span>&#11206;</span> : <span>&#11208;</span>} {this.props.headerText} {this.state.isHovered && (
            <span onClick={this.copyLink} style={{ color: "lightblue" }}># - {this.state.isCopied ? `Copied!` : `Copy link to here`}</span>
          )}
        </HeaderTag>
        {this.state.isOpen && (this.props.children)}
      </>
    );
  }
}
