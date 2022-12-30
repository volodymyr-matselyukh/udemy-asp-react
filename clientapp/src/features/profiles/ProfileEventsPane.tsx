import { Card, Container, Loader, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import ProfileEventCard from "./ProfileEventCard";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import styles from "./ProfileEventsPane.module.css";

interface Props {
	predicate: string;
}

export default observer(function ProfileEventsPane({ predicate }: Props) {
	const {
		profileStore: { loadProfileEvents, profileEvents, loadingEvents },
	} = useStore();

	useEffect(() => {
		loadProfileEvents(predicate);
	}, [loadProfileEvents, predicate]);

	const showCards = () => {
		if (profileEvents) {
			return profileEvents
				.get(predicate)
				?.map((event) => (
					<ProfileEventCard
						key={event.id}
						event={event}
					></ProfileEventCard>
				));
		}
	};

	return (
		<Container fluid className={styles.container}>
			<Card.Group itemsPerRow={4}>
				{loadingEvents ? (
					<div className={styles.loading}>
						<Loader active>Loading events...</Loader>
					</div>
				) : (
					showCards()
				)}
			</Card.Group>
		</Container>
	);
});
