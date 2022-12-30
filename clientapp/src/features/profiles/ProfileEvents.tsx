import { observer } from "mobx-react-lite";
import { Grid, Header, Tab } from "semantic-ui-react";
import ProfileEventsPane from "./ProfileEventsPane";

export default observer(function ProfileEvents() {
	const panes = [
		{menuItem: 'Future Events', render: () => <ProfileEventsPane predicate="future" />},
		{menuItem: 'Past Events', render: () => <ProfileEventsPane predicate="past" />},
		{menuItem: 'Hosting', render: () => <ProfileEventsPane predicate="hosting" />}
	]

	return(
		<Tab.Pane>
			<Grid>
				<Grid.Column width={16}>
					<Header floated="left" icon="calendar" content="Activities" />
				</Grid.Column>
				<Grid.Column width={16}>
					<Tab panes={panes} />
				</Grid.Column>
			</Grid>
		</Tab.Pane>
	);
})